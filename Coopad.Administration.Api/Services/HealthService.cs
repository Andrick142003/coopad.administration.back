using Coopad.Administration.Api.DTOs.Common;
using Coopad.Administration.Api.Models;
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

        public Health GetStatus()
        {
            Health databaseOnline = _healthRepository.GetHealth();

            return databaseOnline;

        }


    }
}
