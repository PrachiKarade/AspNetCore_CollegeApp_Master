using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreWebApiSuperHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoControllerForLog : ControllerBase
    {
        private readonly ILogger<DemoControllerForLog> _logger;
        public DemoControllerForLog(ILogger<DemoControllerForLog> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult GetLogInfo()
        {
            _logger.LogTrace("This is from Trace log level");
            _logger.LogDebug("This is from Debug log level");
            _logger.LogInformation("This is from Information log level");
            _logger.LogWarning("This is from Warning log level");
            _logger.LogError("This is from Error log level");
            _logger.LogCritical("This is from Fetal log level");

            return Ok();
        }
    }
}
