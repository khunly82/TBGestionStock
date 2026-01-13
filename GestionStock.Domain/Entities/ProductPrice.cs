using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Domain.Entities
{
    public class ProductPrice
    {
        public int Id { get; init; }
        public required decimal Price { get; set; }
        public int ProductId { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Product Product { get; set; } = null!;
    }
}
