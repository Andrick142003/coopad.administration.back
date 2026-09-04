using AdoNetCore.AseClient;
using Coopad.Administration.Api.Data;
using Coopad.Administration.Api.DTOs.Requests;
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
                
                .OrderBy(x => x.Semana)
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

        public async Task<CashFlowProjection> UpdateAsync(
            CashFlowProjection projection,
            CancellationToken cancellationToken = default)
        {
            _context.CashFlowProjections.Update(projection);

            await _context.SaveChangesAsync(cancellationToken);

            return projection;
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


        public async Task<CashFlowProjection?> GetByParametersAsync(
        int anio,
        int mes,
        int semana,
        string tipoSaldo,
        string tipo,
        CancellationToken cancellationToken = default)
        {
            return await _context.CashFlowProjections
                .FirstOrDefaultAsync(
                    x =>
                        x.Anio == anio &&
                        x.Mes == mes &&
                        x.Semana == semana &&
                        x.TipoSaldo == tipoSaldo &&
                        x.Tipo == tipo,
                    cancellationToken);
        }



        public  async Task<List<CashFlowValues>> GetCashFlowValuesSP(decimal proyeccion_dp, decimal proyeccion_pf, decimal proyeccion_spi, decimal proyeccion_socios, decimal proyeccion_pa, string tipo, string fecha_inicio, string fecha_fin)
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



        public async Task CreateDateAsync(
            List<CreateCashFlowDateRequest> request, 
            CancellationToken cancellationToken = default)
        {
            foreach (var item in request)
            {
                FechasRango? fecha;

                if (item.Id.HasValue)
                {
                    fecha = await _context.FechasRango
                        .FirstOrDefaultAsync(x => x.Id == item.Id.Value);

                    if (fecha == null)
                    {
                        throw new KeyNotFoundException(
                            $"No existe el registro con Id {item.Id.Value}.");
                    }


                    fecha.Anio = item.Anio;
                    fecha.Mes = item.Mes;
                    fecha.FechaInicio = item.FechaInicio;
                    fecha.FechaFin = item.FechaFin;
                    fecha.Semana = item.Semana;
                }
                else
                {

                    fecha = new FechasRango
                    {
                        Anio = item.Anio,
                        Mes = item.Mes,
                        FechaInicio = item.FechaInicio,
                        FechaFin = item.FechaFin,
                        Semana = item.Semana
                    };

                    await _context.FechasRango.AddAsync(fecha);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task<List<FechasRango>> GetDatesAsync(
        int anio,
        int mes,
        CancellationToken cancellationToken = default)
        {
            var fechas = await _context.FechasRango
                .Where(x => x.Anio == anio && x.Mes == mes)
                .ToListAsync(cancellationToken);

            return fechas;
        }

        public async Task<FechasCashFlow?> GetDatesCoreMovAsync(int anio, int mes, int semana, CancellationToken cancellationToken = default)
        {
            var fechas = await _context.FechasRango
                .Where(x => x.Anio == anio && x.Mes == mes && x.Semana == semana)
                .FirstOrDefaultAsync(cancellationToken);

            if (fechas != null) {
                var fechasObject = new FechasCashFlow
                {

                    FechaInicio = fechas.FechaInicio,
                    FechaFin = fechas.FechaFin,

                };

                return fechasObject;

            }

            return null;


        }

    }
}
