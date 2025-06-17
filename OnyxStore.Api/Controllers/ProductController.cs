using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnyxStore.Api.Dtos;
using OnyxStore.Api.Interfaces;
using OnyxStore.Api.Services;

namespace OnyxStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductsService service) : ControllerBase
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ProductAccess")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Any())
                    .Select(x => new
                    {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage)
                    });

                return BadRequest(new
                {
                    Message = "Validation failed",
                    Details = errors
                });
            }
            var product = await service.CreateAsync(dto);
            return Ok(product);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ProductAccess")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await service.GetAllAsync());

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ProductAccess")]
        [HttpGet("by-color/{color}")]
        public async Task<IActionResult> GetByColor(string color) => Ok(await service.GetByColorAsync(color));
    }
}
