using Coopad.Administration.Api.DTOs.Responses;
using Coopad.Administration.Api.Models;

namespace Coopad.Administration.Api.Repositories.Interfaces
{
    public interface ICashFlowProjectionRepository
    {
        Task<List<CashFlowProjection>> GetAllAsync(
          int anio,
          int mes,
          int semana,
          string tipo,
          CancellationToken cancellationToken = default);

        Task<CashFlowProjection?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<CashFlowProjection> CreateAsync(
            CashFlowProjection projection,
            CancellationToken cancellationToken = default);

        Task<CashFlowProjection?> UpdateAsync(
            CashFlowProjection projection,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            int anio,
            int mes,
            DateTime fechaInicio,
            DateTime fechaFin,
            int semana,
            string tipoSaldo,
            string tipo,
            CancellationToken cancellationToken = default);

        Task<List<CashFlowValues>> GetCashFlowValuesSP
            (
            decimal proyeccion_dp, 
            decimal proyeccion_pf,
            decimal proyeccion_spi,
            decimal proyeccion_socios, 
            decimal proyeccion_pa,
            string tipo,
            string fecha_inicio,
            string fecha_fin
            );
    }
}
