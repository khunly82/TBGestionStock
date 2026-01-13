using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Domain.Entities
{
    public class Category
    {
        public int Id { get; init; }
        public required string Name { get; set; }

        public List<Product> Products { get; set; } = null!;

    }
}
