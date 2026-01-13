using GestionStock.API.Data;
using GestionStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStock.API.Services
{
    public class CategoryService(StockContext db)
    {

        /// <summary>
        /// Récupérer les categories dont l'id se trouve dans ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<Category> GetByIds(List<int> ids)
        {
            return db.Categories.Where(c => ids.Contains(c.Id)).ToList();
        }

        public List<Category> Get()
        {
            return db.Categories.ToList();
        }
    }
}
