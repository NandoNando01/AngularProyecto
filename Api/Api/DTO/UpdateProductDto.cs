using System.ComponentModel.DataAnnotations;

namespace Api.DTO
{
  public class UpdateProductDto
  {
    [Required(ErrorMessage = "El identificador es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El identificador debe ser positivo.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres.")]
    public string? Description { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a cero.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a cero.")]
    public int Stock { get; set; }

    public bool IsActive { get; set; }
  }
}
