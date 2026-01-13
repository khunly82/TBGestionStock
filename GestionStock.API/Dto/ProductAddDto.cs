using System.ComponentModel.DataAnnotations;

namespace GestionStock.API.Dto
{
    public class ProductAddDto
    {
        [Required]
        [MinLength(4)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }
        public int Stock { get; set; }
        public List<int> Categories { get; set; } = null!;
    }
}
