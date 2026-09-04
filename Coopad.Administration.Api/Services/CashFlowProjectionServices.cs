using Coopad.Administration.Api.DTOs.Requests;
using Coopad.Administration.Api.DTOs.Responses;
using Coopad.Administration.Api.Models;
using Coopad.Administration.Api.Repositories.Interfaces;
using Coopad.Administration.Api.Services.Interfaces;

namespace Coopad.Administration.Api.Services
{


    public class CashFlowProjectionService
        : ICashFlowProjectionService
    {
        private readonly ICashFlowProjectionRepository _repository;

        public CashFlowProjectionService(
            ICashFlowProjectionRepository repository)
        {
            _repository = repository;
        }


        public async Task<List<CashFlowProjectionResponse>> GetAllAsync(
            int anio,
            int mes,
            int semana,
            string tipo,
            CancellationToken cancellationToken = default)
        {
            var projections = await _repository.GetAllAsync(
                anio,
                mes,
                semana,
                tipo,
                cancellationToken);

            return projections
                .Select(MapToResponse)
                .ToList();
        }



        public async Task<CashFlowProjectionResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var projection = await _repository.GetByIdAsync(
                id,
                cancellationToken);

            if (projection is null)
            {
                return null;
            }

            return MapToResponse(projection);
        }



        public async Task<CashFlowProjectionResponse> CreateAsync(
            CreateCashFlowProjectionRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request.FechaInicio > request.FechaFin)
            {
                throw new ArgumentException(
                    "La fecha de inicio no puede ser mayor a la fecha de fin.");
            }

            if (request.Proyeccion <= 0)
            {
                throw new ArgumentException(
                    "La proyección debe ser mayor a cero.");
            }

            if (string.IsNullOrWhiteSpace(request.TipoSaldo))
            {
                throw new ArgumentException(
                    "Debe seleccionar un tipo de saldo.");
            }

            if (string.IsNullOrWhiteSpace(request.Tipo))
            {
                throw new ArgumentException(
                    "Debe seleccionar un tipo.");
            }

            var projection = await _repository.GetByParametersAsync(
                request.Anio,
                request.Mes,
                request.Semana,
                request.TipoSaldo,
                request.Tipo,
                cancellationToken);

            if (projection != null)
            {
                projection.Proyeccion = request.Proyeccion;

                var updated = await _repository.UpdateAsync(
                    projection,
                    cancellationToken);

                return MapToResponse(updated);
            }

            projection = new CashFlowProjection
            {
                Anio = request.Anio,
                Mes = request.Mes,
                Semana = request.Semana,
                TipoSaldo = request.TipoSaldo,
                Tipo = request.Tipo,
                Proyeccion = request.Proyeccion,
                CreatedAt = DateTime.Now
            };

            var created = await _repository.CreateAsync(
                projection,
                cancellationToken);

            return MapToResponse(created);
        }


        public async Task<CashFlowProjectionResponse?> UpdateAsync(
            int id,
            UpdateCashFlowProjectionRequest request,
            CancellationToken cancellationToken = default)
        {
            var existing = await _repository.GetByIdAsync(
                id,
                cancellationToken);

            if (existing is null)
            {
                return null;
            }


            if (request.FechaInicio > request.FechaFin)
            {
                throw new ArgumentException(
                    "La fecha de inicio no puede ser mayor a la fecha de fin.");
            }

            if (request.Proyeccion <= 0)
            {
                throw new ArgumentException(
                    "La proyección debe ser mayor a cero.");
            }


            existing.Anio = request.Anio;
            existing.Mes = request.Mes;

            existing.Semana = request.Semana;
            existing.TipoSaldo = request.TipoSaldo;
            existing.Tipo = request.Tipo;
            existing.Proyeccion = request.Proyeccion;
            existing.UpdatedAt = DateTime.Now;


            var updated = await _repository.UpdateAsync(
                existing,
                cancellationToken);

            if (updated is null)
            {
                return null;
            }

            return MapToResponse(updated);
        }



        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _repository.DeleteAsync(
                id,
                cancellationToken);
        }



        private static CashFlowProjectionResponse MapToResponse(
            CashFlowProjection projection)
        {
            return new CashFlowProjectionResponse
            {
                Id = projection.Id,
                Anio = projection.Anio,
                Mes = projection.Mes,
                Semana = projection.Semana,
                TipoSaldo = projection.TipoSaldo,
                Tipo = projection.Tipo,
                Proyeccion = projection.Proyeccion,

            };
        }



        public async Task<List<CashFlowValues>> GetCashFlowSp(
            decimal proyeccion_dp,
            decimal proyeccion_pf,
            decimal proyeccion_spi,
            decimal proyeccion_socios,
            decimal proyeccion_pa,
            string tipo,
            string fecha_inicio,
            string fecha_fin)
        {
            var cashFlowValues = await _repository.GetCashFlowValuesSP(proyeccion_dp, proyeccion_pf, proyeccion_spi, proyeccion_socios, proyeccion_pa, tipo, fecha_inicio, fecha_fin);

            return cashFlowValues;

        }


        public async Task CreateDateAsync(
        List<CreateCashFlowDateRequest> request)
        {
            if (request == null || request.Count == 0)
            {
                throw new ArgumentException(
                    "Debe existir al menos una semana.");
            }

            if (request.Count != 4)
            {
                throw new ArgumentException(
                    "Debe enviar las cuatro semanas.");
            }

            await _repository.CreateDateAsync(request);
        }



        public async Task<List<FechasRango>> GetDatesAsync(
        int anio,
        int mes,
        CancellationToken cancellationToken = default)
        {
            return await _repository.GetDatesAsync(
                anio,
                mes,
                cancellationToken);


        }

        public async Task<CashFlowProjectionResponse?> GetAsync(
        int anio,
        int mes,
        int semana,
        string tipoSaldo,
        string tipo,
        CancellationToken cancellationToken = default)
        {
            var projection = await _repository.GetByParametersAsync(
                anio,
                mes,
                semana,
                tipoSaldo,
                tipo,
                cancellationToken);

            if (projection == null)
            {
                return null;
            }

            return MapToResponse(projection);
        }



        public async Task<FechasCashFlow?> GetDatesCoreMovAsync(
        int anio,
        int mes,
        int semana,
        CancellationToken cancellationToken = default)
        {
            return await _repository.GetDatesCoreMovAsync(
                anio,
                mes,
                semana,
                cancellationToken);


        }
    }
}
