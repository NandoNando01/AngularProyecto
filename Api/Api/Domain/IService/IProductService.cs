using Api.DTO;
using Api.Utils;

namespace Api.Domain.IService
{
  public interface IProductService
  {
    Task<ApiResponse<IEnumerable<ProductDto>>> GetAllAsync();
    Task<ApiResponse<ProductDto>> GetByIdAsync(int id);
    Task<ApiResponse<ProductDto>> CreateAsync(CreateProductDto dto);
    Task<ApiResponse<ProductDto>> UpdateAsync(UpdateProductDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
  }
}
