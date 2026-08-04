using Coopad.Administration.Api.DTOs.Common;
using Coopad.Administration.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Coopad.Administration.Api.Services
{
    public class HealthService : IHealthService
    {
        
        public HealthResponse GetStatus() {

            var response = new HealthResponse
            {

                Message = "Api funciona",
                Timestamp = DateTime.UtcNow,
                Version = "1.0"

            };

            return response;
            
        }


    }
}
