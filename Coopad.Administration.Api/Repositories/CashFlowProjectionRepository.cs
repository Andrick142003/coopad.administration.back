using AdoNetCore.AseClient;
using Coopad.Administration.Api.Data;
using Coopad.Administration.Api.DTOs.Responses;
using Coopad.Administration.Api.Infrastructure.Database;
using Coopad.Administration.Api.Models;
using Coopad.Administration.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Coopad.Administration.Api.Repositories
{
    public class CashFlowProjectionRepository : ICashFlowProjectionRepository
    {
        private readonly SecurityDbContext _context;
        private readonly IAseConnectionFactory _connectionFactory;

        public CashFlowProjectionRepository(SecurityDbContext context, IAseConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        public async Task<List<CashFlowProjection>> GetAllAsync(
            int anio,
            int mes,
            int semana,
            string tipo,
            CancellationToken cancellationToken = default)
        {
            return await _context.CashFlowProjections
                .AsNoTracking()
                .Where(x =>
                    x.Anio == anio &&
                    x.Mes == mes &&
                    x.Semana == semana &&
                    x.Tipo == tipo)
                .OrderBy(x => x.FechaInicio)
                .ThenBy(x => x.Semana)
                .ThenBy(x => x.Tipo)
                .ToListAsync(cancellationToken);
        }

        public async Task<CashFlowProjection?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.CashFlowProjections
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }


        public async Task<CashFlowProjection> CreateAsync(
            CashFlowProjection projection,
            CancellationToken cancellationToken = default)
        {
            await _context.CashFlowProjections.AddAsync(
                projection,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return projection;
        }


        public async Task<CashFlowProjection?> UpdateAsync(
            CashFlowProjection projection,
            CancellationToken cancellationToken = default)
        {
            var existing = await _context.CashFlowProjections
                .FirstOrDefaultAsync(
                    x => x.Id == projection.Id,
                    cancellationToken);

            if (existing is null)
            {
                return null;
            }

            existing.Anio = projection.Anio;
            existing.Mes = projection.Mes;
            existing.FechaInicio = projection.FechaInicio;
            existing.FechaFin = projection.FechaFin;
            existing.Semana = projection.Semana;
            existing.TipoSaldo = projection.TipoSaldo;
            existing.Tipo = projection.Tipo;
            existing.Proyeccion = projection.Proyeccion;
            existing.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return existing;
        }



        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var projection = await _context.CashFlowProjections
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);

            if (projection is null)
            {
                return false;
            }

            _context.CashFlowProjections.Remove(projection);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }


        public async Task<bool> ExistsAsync(
            int anio,
            int mes,
            DateTime fechaInicio,
            DateTime fechaFin,
            int semana,
            string tipoSaldo,
            string tipo,
            CancellationToken cancellationToken = default)
        {
            return await _context.CashFlowProjections
                .AsNoTracking()
                .AnyAsync(
                    x =>
                        x.Anio == anio &&
                        x.Mes == mes &&
                        x.FechaInicio == fechaInicio &&
                        x.FechaFin == fechaFin &&
                        x.Semana == semana &&
                        x.TipoSaldo == tipoSaldo &&
                        x.Tipo == tipo,
                    cancellationToken);
        }



        public async Task<List<CashFlowValues>> GetCashFlowValuesSP(decimal proyeccion_dp, decimal proyeccion_pf, decimal proyeccion_spi, decimal proyeccion_socios, decimal proyeccion_pa, string tipo, string fecha_inicio, string fecha_fin)
        {


            var result = new List<CashFlowValues>();

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();


            command.CommandText = "cob_credito_his..sp_flujo_caja_consolidado";
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.Add(new AseParameter("@i_tipo", tipo));
            command.Parameters.Add(new AseParameter("@i_planificado_dp", proyeccion_dp));
            command.Parameters.Add(new AseParameter("@i_planificado_pf", proyeccion_pf));
            command.Parameters.Add(new AseParameter("@i_planificado_spi", proyeccion_spi));
            command.Parameters.Add(new AseParameter("@i_planificado_socios", proyeccion_socios));
            command.Parameters.Add(new AseParameter("@i_planificado_pa", proyeccion_pa));
            command.Parameters.Add(new AseParameter("@i_fecha_ini", fecha_inicio));
            command.Parameters.Add(new AseParameter("@i_fecha_fin", fecha_fin));

            using var reader = command.ExecuteReader();

            while (reader.Read()) {
                var item = new CashFlowValues
                {

                    codigo = reader["codigo"] is null ? 0 : Convert.ToInt32(reader["codigo"]),
                    descripcion = reader["descripcion"]?.ToString()?.Trim() ?? "",
                    proyectado = reader["proyectado"] is null ? 0 : Convert.ToDecimal(reader["proyectado"]),
                    valor = reader["valor"] is null ? 0 : Convert.ToDecimal(reader["valor"]),
                    variacion = reader["variacion"] is null ? 0 : Convert.ToDecimal(reader["variacion"]),
                    variacionPorcentual = reader["variacion_porcentual"] is null ? 0 : Convert.ToDecimal(reader["variacion_porcentual"])

                };

                result.Add(item);

            }


           return result;

        }

    }
}
