using GestionStock.API.Dto;
using GestionStock.API.Hubs;
using GestionStock.API.Services;
using GestionStock.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GestionStock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(
        ProductService productService,
        CategoryService categoryService,
        IHubContext<ProductHub> productHub
    ) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<ProductDto>> Get()
        {
            return Ok(productService.Get()
                .Select(p => new ProductDto(p))
            );
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductAddDto dto)
        {

            try
            {
                using var stream = dto.Image?.OpenReadStream();
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
                }, stream);
                await productHub.Clients.All.SendAsync("productsHasChanged");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                productService.Remove(id);
                await productHub.Clients.All.SendAsync("productsHasChanged");
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
