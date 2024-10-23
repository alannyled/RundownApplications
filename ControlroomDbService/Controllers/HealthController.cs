using Microsoft.AspNetCore.Mvc;

namespace ControlRoomDbService.Controllers
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
