using Coopad.Administration.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coopad.Administration.Api.Controllers;

[ApiController]
[Route("api/sorteo")]
public sealed class SorteoController : ControllerBase
{
    private readonly ISorteoService _service;

    public SorteoController(ISorteoService service)
    {
        _service = service;
    }

    [HttpGet("numeros")]
    public async Task<IActionResult> ObtenerNumeros(
        [FromQuery] string tipo,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.ObtenerNumerosAsync(
                tipo,
                cancellationToken);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("numeros/{id:int}/estado")]
    public async Task<IActionResult> ActualizarEstado(
        int id,
        [FromBody] ActualizarEstadoRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ActualizarEstadoAsync(
            id,
            request.Estado,
            cancellationToken);

        if (result is null)
        {
            return NotFound(new
            {
                message = "El número no existe."
            });
        }

        return Ok(result);
    }

    [HttpPost("realizar")]
    public async Task<IActionResult> RealizarSorteo(
        [FromBody] RealizarSorteoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.RealizarSorteoAsync(
                request.Tipo,
                cancellationToken);

            if (result is null)
            {
                return Conflict(new
                {
                    message = "No existen números habilitados para esta categoría."
                });
            }

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public sealed record ActualizarEstadoRequest(bool Estado);

public sealed record RealizarSorteoRequest(string Tipo);