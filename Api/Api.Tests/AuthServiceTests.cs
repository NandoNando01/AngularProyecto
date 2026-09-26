using Api.Domain.IService;
using Api.Domain.iRepositories;
using Api.Domain.Models;
using Api.DTO;
using Api.Service;
using Api.Utils;
using Microsoft.Extensions.Options;
using Moq;

namespace Api.Tests
{
  public class AuthServiceTests
  {
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IAuthService _service;

    public AuthServiceTests()
    {
      var jwtSettings = Options.Create(new JwtSettings
      {
        Secret = "Test_Secret_Key_That_Is_At_Least_32_Characters_Long",
        Issuer = "Api",
        Audience = "mi-app-test",
        ExpirationMinutes = 120
      });

      _userRepositoryMock = new Mock<IUserRepository>();
      _unitOfWorkMock = new Mock<IUnitOfWork>();
      _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
      _service = new AuthService(_unitOfWorkMock.Object, jwtSettings);
    }

    [Fact]
    public async Task Register_WithNewEmail_AddsUserAndReturnsToken()
    {
      _userRepositoryMock.Setup(r => r.ExistsByEmailAsync("nuevo@example.com")).ReturnsAsync(false);
      var dto = new RegisterDto
      {
        FullName = "Juan Pérez",
        Email = "NUEVO@example.com",
        Password = "Segura#123"
      };

      var result = await _service.RegisterAsync(dto);

      Assert.True(result.Success);
      Assert.Equal("Usuario registrado correctamente.", result.Message);
      Assert.NotNull(result.Data?.Token);
      Assert.True(result.Data?.ExpiresAt > DateTime.UtcNow);
      _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
      _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);

      var storedEmail = result.Data?.Email;
      Assert.Equal("nuevo@example.com", storedEmail);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ThrowsBusinessException()
    {
      _userRepositoryMock.Setup(r => r.ExistsByEmailAsync("existente@example.com")).ReturnsAsync(true);
      var dto = new RegisterDto
      {
        FullName = "María López",
        Email = "existente@example.com",
        Password = "Segura#123"
      };

      await Assert.ThrowsAsync<BusinessException>(() => _service.RegisterAsync(dto));
      _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
      _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Register_HashesPassword()
    {
      _userRepositoryMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
      User? addedUser = null;
      _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>()))
          .Callback<User>(u => addedUser = u)
          .Returns(Task.CompletedTask);
      var dto = new RegisterDto
      {
        FullName = "Ana García",
        Email = "ana@example.com",
        Password = "Segura#123"
      };

      await _service.RegisterAsync(dto);

      Assert.NotNull(addedUser);
      Assert.NotEqual("Segura#123", addedUser!.PasswordHash);
      Assert.True(PasswordHasher.Verify("Segura#123", addedUser!.PasswordHash));
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
      var user = new User
      {
        Id = 1,
        FullName = "Carlos Rodríguez",
        Email = "usuario@test.com",
        PasswordHash = PasswordHasher.Hash("MiContraseña#1")
      };
      _userRepositoryMock.Setup(r => r.GetByEmailAsync("usuario@test.com")).ReturnsAsync(user);
      var dto = new LoginDto
      {
        Email = "usuario@test.com",
        Password = "MiContraseña#1"
      };

      var result = await _service.LoginAsync(dto);

      Assert.True(result.Success);
      Assert.Equal("Inicio de sesión exitoso.", result.Message);
      Assert.Equal("Carlos Rodríguez", result.Data?.FullName);
      Assert.NotNull(result.Data?.Token);
      Assert.Equal(1, result.Data?.Id);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ThrowsUnauthorizedException()
    {
      var user = new User
      {
        Id = 1,
        FullName = "Carlos Rodríguez",
        Email = "usuario@test.com",
        PasswordHash = PasswordHasher.Hash("MiContraseña#1")
      };
      _userRepositoryMock.Setup(r => r.GetByEmailAsync("usuario@test.com")).ReturnsAsync(user);
      var dto = new LoginDto
      {
        Email = "usuario@test.com",
        Password = "contraseña-incorrecta"
      };

      await Assert.ThrowsAsync<UnauthorizedException>(() => _service.LoginAsync(dto));
    }

    [Fact]
    public async Task Login_WithNonExistingEmail_ThrowsUnauthorizedException()
    {
      _userRepositoryMock.Setup(r => r.GetByEmailAsync("noexiste@test.com"))
          .ReturnsAsync((User?)null);
      var dto = new LoginDto
      {
        Email = "noexiste@test.com",
        Password = "MiContraseña#1"
      };

      await Assert.ThrowsAsync<UnauthorizedException>(() => _service.LoginAsync(dto));
    }
  }
}