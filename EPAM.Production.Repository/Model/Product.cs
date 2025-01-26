using System.ComponentModel.DataAnnotations;

namespace EPAM.Production.Repository.Model;

public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Weight must be a non-negative value.")]
    public decimal Weight { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Height must be a non-negative value.")]
    public decimal Height { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Width must be a non-negative value.")]
    public decimal Width { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Length must be a non-negative value.")]
    public decimal Length { get; set; }
}
