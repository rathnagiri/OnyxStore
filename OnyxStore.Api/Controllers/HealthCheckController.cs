using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnyxStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("OK");
    }
}
