using GestionStock.API.Dto;
using GestionStock.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionStock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(CategoryService categoryService) : ControllerBase
    {

        [HttpGet]
        public ActionResult<List<CategoryDto>> Get() 
        {
            return Ok(categoryService.Get().Select(c => new CategoryDto(c)));
        }
    }
}
