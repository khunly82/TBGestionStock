using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Domain.Entities
{
    public class Product
    {
        public int Id { get; init; }
        public required string Reference { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public required int Stock { get; set; }
        public bool IsDeleted { get; set; }

        // navigation Properties
        public List<Category> Categories { get; set; } = null!;
        public List<ProductPrice> Prices { get; set; } = null!;
    }
}
