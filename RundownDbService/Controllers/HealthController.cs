using Microsoft.AspNetCore.Mvc;

namespace RundownDbService.Controllers
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
