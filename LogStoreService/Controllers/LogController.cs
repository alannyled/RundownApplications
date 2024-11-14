using Microsoft.AspNetCore.Mvc;
using LogStoreService.BLL.Interfaces;
using LogStoreService.Models;

namespace LogStoreService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

       
        public class LogController(ILogStoreService logService) : ControllerBase
        {
            private readonly ILogStoreService _logService = logService;

            [HttpGet]
            public async Task<ActionResult<List<Log>>> GetAllLogs()
            {
                var logs = await _logService.GetLogsAsync();
                return Ok(logs);
            }
        [HttpDelete]
        public async Task<ActionResult> DeleteAllLogs()
        {
            await _logService.DeleteAllLogsAsync();
            return Ok();
        }
    }
   
}
