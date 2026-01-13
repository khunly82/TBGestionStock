using GestionStock.Domain.Entities;

namespace GestionStock.API.Dto
{
    public class ProductDto(Product product)
    {
        public int Id { get; set; } = product.Id;
        public string Reference { get; set; } = product.Reference;
        public string Name { get; set; } = product.Name;
        public string? Description { get; set; } = product.Description;
        public string? ImageUrl { get; set; } = product.ImageUrl;
        public decimal Price { get; set; } = product.Prices.First(p => p.EndDate == null).Price;
        public List<CategoryDto> Categories { get; set; } = product.Categories.Select(c => new CategoryDto(c)).ToList();
    }
}
