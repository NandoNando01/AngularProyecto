using Api.Domain.Models;
using Api.DTO;

namespace Api.Utils
{
  public static class ProductMapper
  {
    public static ProductDto ToDto(Product entity) => new()
    {
      Id = entity.Id,
      Name = entity.Name,
      Description = entity.Description,
      Price = entity.Price,
      Stock = entity.Stock,
      IsActive = entity.IsActive,
      CreatedAt = entity.CreatedAt
    };

    public static Product ToEntity(CreateProductDto dto) => new()
    {
      Name = dto.Name,
      Description = dto.Description,
      Price = dto.Price,
      Stock = dto.Stock,
      IsActive = dto.IsActive
    };

    public static void ApplyTo(UpdateProductDto dto, Product entity)
    {
      entity.Name = dto.Name;
      entity.Description = dto.Description;
      entity.Price = dto.Price;
      entity.Stock = dto.Stock;
      entity.IsActive = dto.IsActive;
    }
  }
}
