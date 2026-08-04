using Coopad.Administration.Api.DTOs.Common;
using Coopad.Administration.Api.Repositories.Interfaces;
using Coopad.Administration.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Coopad.Administration.Api.Services
{
    public class HealthService : IHealthService
    {

        private readonly IHealthRepository _healthRepository;

        public HealthService(IHealthRepository healthRepository)
        {
            _healthRepository = healthRepository;
        }

        public HealthResponse GetStatus()
        {
            bool databaseOnline = _healthRepository.IsDatabaseAvailable();
            var response = new HealthResponse
            {
                Message = databaseOnline ? "API funcionando correctamente" : "La base de datos no está disponible",

                Timestamp = DateTime.UtcNow,
                Version = "1.0"

            };

            return response;

        }


    }
}
