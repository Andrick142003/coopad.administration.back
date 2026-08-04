using Coopad.Administration.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Coopad.Administration.Api.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class HealthController :  ControllerBase
    {
        private readonly IHealthService _healthService;



        public HealthController(IHealthService healthService) { 
        
        _healthService = healthService;
        }




        [HttpGet]
        public IActionResult Get()
        {
            var response = _healthService.GetStatus();

            return Ok(response);
        }
    }
}
