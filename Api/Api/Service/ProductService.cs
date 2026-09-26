using Api.Domain.IService;
using Api.Domain.iRepositories;
using Api.Domain.Models;
using Api.DTO;
using Api.Utils;

namespace Api.Service
{
  public class ProductService : IProductService
  {
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>> GetAllAsync()
    {
      var products = await _unitOfWork.Products.GetAllAsync();
      var result = products.Select(ProductMapper.ToDto);
      return ApiResponse<IEnumerable<ProductDto>>.Ok(result);
    }

    public async Task<ApiResponse<ProductDto>> GetByIdAsync(int id)
    {
      var product = await GetProductOrThrowAsync(id);
      return ApiResponse<ProductDto>.Ok(ProductMapper.ToDto(product));
    }

    public async Task<ApiResponse<ProductDto>> CreateAsync(CreateProductDto dto)
    {
      ValidateBusinessRules(dto.Price, dto.Stock);

      var entity = ProductMapper.ToEntity(dto);
      await _unitOfWork.Products.AddAsync(entity);
      await _unitOfWork.CompleteAsync();

      return ApiResponse<ProductDto>.Ok(ProductMapper.ToDto(entity), "Producto creado correctamente.");
    }

    public async Task<ApiResponse<ProductDto>> UpdateAsync(UpdateProductDto dto)
    {
      ValidateBusinessRules(dto.Price, dto.Stock);

      var product = await GetProductOrThrowAsync(dto.Id);
      ProductMapper.ApplyTo(dto, product);
      _unitOfWork.Products.Update(product);
      await _unitOfWork.CompleteAsync();

      return ApiResponse<ProductDto>.Ok(ProductMapper.ToDto(product), "Producto actualizado correctamente.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
      var product = await GetProductOrThrowAsync(id);
      _unitOfWork.Products.Remove(product);
      await _unitOfWork.CompleteAsync();

      return ApiResponse<bool>.Ok(true, "Producto eliminado correctamente.");
    }

    private async Task<Product> GetProductOrThrowAsync(int id)
    {
      var product = await _unitOfWork.Products.GetByIdAsync(id);
      if (product is null)
      {
        throw new NotFoundException($"No se encontró el producto con Id {id}.");
      }
      return product;
    }

    private static void ValidateBusinessRules(decimal price, int stock)
    {
      if (price < 0)
      {
        throw new BusinessException("El precio no puede ser negativo.");
      }
      if (stock < 0)
      {
        throw new BusinessException("El stock no puede ser negativo.");
      }
    }
  }
}
