using System.ComponentModel.DataAnnotations;

namespace ProductManager.Models;

public class Product : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Enter a product name.")]
    [StringLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "99999999", ErrorMessage = "Enter a price greater than zero.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Enter a category.")]
    [StringLength(60, ErrorMessage = "The category cannot exceed 60 characters.")]
    public string Category { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stock { get; set; }

    [StringLength(1000, ErrorMessage = "The description cannot exceed 1,000 characters.")]
    public string? Description { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Price != decimal.Round(Price, 2))
        {
            yield return new ValidationResult(
                "Enter a price with no more than two decimal places.",
                [nameof(Price)]);
        }
    }
}
