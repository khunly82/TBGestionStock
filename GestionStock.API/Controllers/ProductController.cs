using GestionStock.API.Dto;
using GestionStock.API.Services;
using GestionStock.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionStock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(
        ProductService productService,
        CategoryService categoryService
    ) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<ProductDto>> Get()
        {
            return Ok(productService.Get().Select(p => new ProductDto(p)));
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductAddDto dto)
        {
            using MemoryStream ms = new MemoryStream();
            dto.Image?.CopyTo(ms);
            ms.Position = 0;

            try
            {
                Product p = await productService.Add(new Product
                {
                    Reference = "",
                    Name = dto.Name,
                    Description = dto.Description,
                    Categories = categoryService.GetByIds(dto.Categories),
                    Stock = dto.Stock,
                    Prices = [
                            new ProductPrice {
                        Price = dto.Price,
                        StartDate = DateTime.UtcNow
                    }
                        ]
                }, dto.Image == null ? null : ms);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created();
        }
    }
}
