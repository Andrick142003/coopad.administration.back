using Coopad.Administration.Api.DTOs.Requests;
using Coopad.Administration.Api.DTOs.Responses;
using Coopad.Administration.Api.Models;

namespace Coopad.Administration.Api.Services.Interfaces
{
    public interface ICashFlowProjectionService
    {
        Task<List<CashFlowProjectionResponse>> GetAllAsync(
     int anio,
     int mes,
     int semana,
     string tipo,
     CancellationToken cancellationToken = default);

        Task<CashFlowProjectionResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<CashFlowProjectionResponse?> UpdateAsync(int id, UpdateCashFlowProjectionRequest request, CancellationToken cancellationToken = default);

        Task<CashFlowProjectionResponse> CreateAsync(
            CreateCashFlowProjectionRequest request,
            CancellationToken cancellationToken = default);



        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    


            Task<List<CashFlowValues>> GetCashFlowSp
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


        Task CreateDateAsync(List<CreateCashFlowDateRequest> request);



        Task<List<FechasRango>> GetDatesAsync(
        int anio,
        int mes,
        CancellationToken cancellationToken = default);


        Task<CashFlowProjectionResponse?> GetAsync(
        int anio,
        int mes,
        int semana,
        string tipoSaldo,
        string tipo,
        CancellationToken cancellationToken = default);


        Task<FechasCashFlow?> GetDatesCoreMovAsync(
        int anio,
        int mes,
        int semana,
        CancellationToken cancellationToken = default);


    }


}
