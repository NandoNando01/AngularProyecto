using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Domain.IService;
using Api.Domain.iRepositories;
using Api.Domain.Models;
using Api.DTO;
using Api.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Service
{
  public class AuthService : IAuthService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettings)
    {
      _unitOfWork = unitOfWork;
      _jwtSettings = jwtSettings.Value;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
      if (await _unitOfWork.Users.ExistsByEmailAsync(dto.Email))
      {
        throw new BusinessException("El correo electrónico ya está registrado.");
      }

      var user = new User
      {
        FullName = dto.FullName.Trim(),
        Email = dto.Email.Trim().ToLowerInvariant(),
        PasswordHash = PasswordHasher.Hash(dto.Password)
      };

      await _unitOfWork.Users.AddAsync(user);
      await _unitOfWork.CompleteAsync();

      var response = BuildAuthResponse(user, "Usuario registrado correctamente.");
      return response;
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
      var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email.Trim().ToLowerInvariant());
      if (user is null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
      {
        throw new UnauthorizedException("Credenciales inválidas.");
      }

      var response = BuildAuthResponse(user, "Inicio de sesión exitoso.");
      return response;
    }

    private ApiResponse<AuthResponseDto> BuildAuthResponse(User user, string message)
    {
      var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);
      var data = new AuthResponseDto
      {
        Id = user.Id,
        Email = user.Email,
        FullName = user.FullName,
        Token = GenerateToken(user, expiresAt),
        ExpiresAt = expiresAt
      };

      return ApiResponse<AuthResponseDto>.Ok(data, message);
    }

    private string GenerateToken(User user, DateTime expiresAt)
    {
      var claims = new List<Claim>
      {
        new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new(JwtRegisteredClaimNames.Email, user.Email),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new("fullName", user.FullName)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: claims,
        expires: expiresAt,
        signingCredentials: credentials);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}