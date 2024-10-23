using Microsoft.AspNetCore.Mvc;

namespace TemplateDbService.Controllers
{
   
        [ApiController]
        [Route("[controller]")]
        public class HealthController : ControllerBase
        {
            [HttpGet]
            public IActionResult GetHealth()
            {
                return Ok("ok");
            }
        }

}
