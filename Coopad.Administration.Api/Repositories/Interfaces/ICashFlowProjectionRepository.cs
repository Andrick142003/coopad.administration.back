using Coopad.Administration.Api.DTOs.Requests;
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

        Task<CashFlowProjection?> GetByParametersAsync(
        int anio,
        int mes,
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


        Task CreateDateAsync(
            List<CreateCashFlowDateRequest> request,
            CancellationToken cancellationToken = default);



        Task<List<FechasRango>> GetDatesAsync(
        int anio,
        int mes,
        CancellationToken cancellationToken = default);


        Task<FechasCashFlow?> GetDatesCoreMovAsync(int anio, int mes, int semana, CancellationToken cancellationToken = default);

    }
}
