using Coopad.Administration.Api.DTOs.Requests;
using Coopad.Administration.Api.DTOs.Responses;
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


            var depositosVista = projections
            .FirstOrDefault(x => x.TipoSaldo == "depositos_vista");

            var depositosPf = projections
            .FirstOrDefault(x => x.TipoSaldo == "depositos_pf");



            Console.WriteLine(depositosVista);



            var proyeccion_dp = depositosVista.Proyeccion;
            var proyeccion_pf = depositosPf.Proyeccion;
            var proyeccion_spi = 300000;
            var proyeccion_socios = 500000;
            var proyeccion_pa = 250000;
            var fecha_inicio = depositosVista.FechaInicio.ToString("MM/dd/yyyy");
            var fecha_fin = depositosVista.FechaFin.ToString("MM/dd/yyyy");



            var cash_flow_async  = await _service.GetCashFlowSp(proyeccion_dp, proyeccion_pf, proyeccion_spi, proyeccion_socios, proyeccion_pa, tipo, fecha_inicio, fecha_fin);


            return Ok(cash_flow_async);
        }

    }
}
