using GestionStock.Domain.Entities;

namespace GestionStock.API.Dto
{
    public class CategoryDto(Category category)
    {
        public int Id { get; set; } = category.Id;
        public string Name { get; set; } = category.Name;
    }
}
