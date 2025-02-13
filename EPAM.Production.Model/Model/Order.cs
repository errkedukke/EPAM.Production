using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAM.Production.Repository.Model;

public class Order
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Status Status { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Column(TypeName = "datetime2")]
    public DateTime? UpdatedDate { get; set; } = null;

    [Required]
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;
}
