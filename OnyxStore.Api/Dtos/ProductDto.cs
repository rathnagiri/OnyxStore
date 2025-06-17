using System.ComponentModel.DataAnnotations;

namespace OnyxStore.Api.Dtos;

public class ProductDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(25)]
    public string Name { get; set; }
    
    [Required]
    [MinLength(3)]
    public string Color { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}