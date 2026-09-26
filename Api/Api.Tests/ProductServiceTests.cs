using Api.Domain.IService;
using Api.Domain.iRepositories;
using Api.Domain.Models;
using Api.DTO;
using Api.Service;
using Api.Utils;
using Moq;

namespace Api.Tests
{
  public class ProductServiceTests
  {
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IProductService _service;

    public ProductServiceTests()
    {
      _productRepositoryMock = new Mock<IProductRepository>();
      _unitOfWorkMock = new Mock<IUnitOfWork>();
      _unitOfWorkMock.Setup(u => u.Products).Returns(_productRepositoryMock.Object);
      _service = new ProductService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsAllProducts()
    {
      var products = new List<Product>
      {
        new() { Id = 1, Name = "Producto 1", Price = 10, Stock = 5 },
        new() { Id = 2, Name = "Producto 2", Price = 20, Stock = 8 }
      };
      _productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

      var result = await _service.GetAllAsync();

      Assert.True(result.Success);
      Assert.Equal(2, result.Data?.Count());
      _productRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetById_WithExistingId_ReturnsProduct()
    {
      var product = new Product { Id = 1, Name = "Producto", Price = 10, Stock = 5 };
      _productRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

      var result = await _service.GetByIdAsync(1);

      Assert.True(result.Success);
      Assert.Equal("Producto", result.Data?.Name);
    }

    [Fact]
    public async Task GetById_WithNonExistingId_ThrowsNotFoundException()
    {
      _productRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

      await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(999));
    }

    [Fact]
    public async Task Create_WithValidData_AddsAndReturnsProduct()
    {
      var dto = new CreateProductDto { Name = "Nuevo", Price = 10, Stock = 5 };

      var result = await _service.CreateAsync(dto);

      Assert.True(result.Success);
      Assert.Equal("Nuevo", result.Data?.Name);
      _productRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
      _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Create_WithNegativePrice_ThrowsBusinessException()
    {
      var dto = new CreateProductDto { Name = "Invalido", Price = -1, Stock = 5 };

      await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(dto));
      _productRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
      _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Create_WithNegativeStock_ThrowsBusinessException()
    {
      var dto = new CreateProductDto { Name = "Invalido", Price = 10, Stock = -1 };

      await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(dto));
      _productRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task Update_WithExistingId_UpdatesProduct()
    {
      var product = new Product { Id = 1, Name = "Original", Price = 10, Stock = 5 };
      _productRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
      var dto = new UpdateProductDto { Id = 1, Name = "Actualizado", Price = 15, Stock = 6 };

      var result = await _service.UpdateAsync(dto);

      Assert.True(result.Success);
      Assert.Equal("Actualizado", result.Data?.Name);
      Assert.Equal(15, result.Data?.Price);
      _productRepositoryMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
      _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_WithNonExistingId_ThrowsNotFoundException()
    {
      _productRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);
      var dto = new UpdateProductDto { Id = 1, Name = "X", Price = 10, Stock = 5 };

      await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(dto));
      _productRepositoryMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task Delete_WithExistingId_RemovesProduct()
    {
      var product = new Product { Id = 1, Name = "Producto" };
      _productRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

      var result = await _service.DeleteAsync(1);

      Assert.True(result.Success);
      Assert.True(result.Data);
      _productRepositoryMock.Verify(r => r.Remove(It.IsAny<Product>()), Times.Once);
      _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_WithNonExistingId_ThrowsNotFoundException()
    {
      _productRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

      await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(1));
      _productRepositoryMock.Verify(r => r.Remove(It.IsAny<Product>()), Times.Never);
    }
  }
}