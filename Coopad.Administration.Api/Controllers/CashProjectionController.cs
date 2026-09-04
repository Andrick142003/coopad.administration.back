using Coopad.Administration.Api.DTOs.Requests;
using Coopad.Administration.Api.DTOs.Responses;
using Coopad.Administration.Api.Models;
using Coopad.Administration.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coopad.Administration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashFlowProjectionController : ControllerBase
    {
        private readonly ICashFlowProjectionService _service;

        public CashFlowProjectionController(
            ICashFlowProjectionService service)
        {
            _service = service;
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult<CashFlowProjectionResponse>> GetById(
            int id)
        {
            var projection = await _service.GetByIdAsync(id);

            if (projection == null)
            {
                return NotFound(new
                {
                    message = "No se encontró la proyección de flujo de caja."
                });
            }

            return Ok(projection);
        }


        [HttpPost]
        public async Task<ActionResult<CashFlowProjectionResponse>> Create(
            CreateCashFlowProjectionRequest request)
        {
            var projection = await _service.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = projection.Id },
                projection);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CashFlowProjectionResponse>> Update(
            int id,
            UpdateCashFlowProjectionRequest request)
        {
            var projection = await _service.UpdateAsync(id, request);

            if (projection == null)
            {
                return NotFound(new
                {
                    message = "No se encontró la proyección de flujo de caja."
                });
            }

            return Ok(projection);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "No se encontró la proyección de flujo de caja."
                });
            }

            return NoContent();
        }



        [HttpGet]
        public async Task<ActionResult> getCashflow(int anio, int mes, int semana, string tipo)
        {
            var projections = await _service.GetAllAsync(anio, mes, semana, tipo);

            var fechas = await _service.GetDatesCoreMovAsync(anio, mes, semana);

            var depositosVista = projections
                .FirstOrDefault(x => x.TipoSaldo == "depositos_vista");

            var depositosPf = projections
                .FirstOrDefault(x => x.TipoSaldo == "depositos_pf");

            var transferenciasSpi = projections
                .FirstOrDefault(x => x.TipoSaldo == "transferencia_spi");

            var aporteSocios = projections
                .FirstOrDefault(x => x.TipoSaldo == "aporte_socios");

            var pagoAgil = projections
                .FirstOrDefault(x => x.TipoSaldo == "pago_agil");

            var proyeccion_dp = depositosVista?.Proyeccion ?? 0;
            var proyeccion_pf = depositosPf?.Proyeccion ?? 0;
            var proyeccion_spi = transferenciasSpi?.Proyeccion ?? 0;
            var proyeccion_socios = aporteSocios?.Proyeccion ?? 0;
            var proyeccion_pa = pagoAgil?.Proyeccion ?? 0;

            var fecha_inicio = fechas?.FechaInicio?
                .ToString("MM/dd/yyyy") ?? "01/01/1900";

            var fecha_fin = fechas?.FechaFin?
                .ToString("MM/dd/yyyy") ?? "01/01/1900";

            var cash_flow_async = await _service.GetCashFlowSp(
                proyeccion_dp,
                proyeccion_pf,
                proyeccion_spi,
                proyeccion_socios,
                proyeccion_pa,
                tipo,
                fecha_inicio,
                fecha_fin
            );

            return Ok(cash_flow_async);
        }





        [HttpPost("createDates")]
        public async Task<IActionResult> Create(
         [FromBody] List<CreateCashFlowDateRequest> request)
        {
            await _service.CreateDateAsync(request);

            return Ok();
        }



        [HttpGet("ListFechas")]
        public async Task<IActionResult> GetDates(
        [FromQuery] int anio,
        [FromQuery] int mes,
        CancellationToken cancellationToken)
        {
            var result = await _service.GetDatesAsync(
                anio,
                mes,
                cancellationToken);

            return Ok(result);
        }





        [HttpGet("ListCashFlowRegister")]
        public async Task<IActionResult> Get(
        [FromQuery] int anio,
        [FromQuery] int mes,
        [FromQuery] int semana,
        [FromQuery] string tipoSaldo,
        [FromQuery] string tipo,
        CancellationToken cancellationToken)
        {
            var result = await _service.GetAsync(
                anio,
                mes,
                semana,
                tipoSaldo,
                tipo,
                cancellationToken);

            return Ok(result);
        }


    }
}
