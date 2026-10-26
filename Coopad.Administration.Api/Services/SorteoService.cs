using Coopad.Administration.Api.DTOs.Responses;
using Coopad.Administration.Api.Repositories;
using Coopad.Administration.Api.Repositories.Interfaces;
using Coopad.Administration.Api.Services.Interfaces;

namespace Coopad.Administration.Api.Services;

public sealed class SorteoService : ISorteoService
{
    private readonly ISorteoRepository _repository;

    public SorteoService(ISorteoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SorteoNumeroResponse>> ObtenerNumerosAsync(
        string tipo,
        CancellationToken cancellationToken = default)
    {
        tipo = tipo.Trim().ToUpperInvariant();

        ValidarTipo(tipo);

        var numeros = await _repository.ObtenerPorTipoAsync(
            tipo,
            cancellationToken);

        return numeros
            .Select(x => new SorteoNumeroResponse(
                x.Id,
                x.Numero,
                x.Tipo,
                x.Estado))
            .ToList();
    }

    public async Task<SorteoNumeroResponse?> ActualizarEstadoAsync(
        int id,
        bool estado,
        CancellationToken cancellationToken = default)
    {
        var numero = await _repository.ObtenerPorIdAsync(
            id,
            cancellationToken);

        if (numero is null)
            return null;

        numero.Estado = estado;

        await _repository.ActualizarEstadoAsync(
            numero,
            cancellationToken);

        return new SorteoNumeroResponse(
            numero.Id,
            numero.Numero,
            numero.Tipo,
            numero.Estado);
    }

    private static void ValidarTipo(string tipo)
    {
        if (tipo is not ("S" or "I"))
        {
            throw new ArgumentException(
                "El tipo debe ser S (Socios) o I (Instituciones).",
                nameof(tipo));
        }
    }



    public async Task<SorteoNumeroResponse?> RealizarSorteoAsync(
    string tipo,
    CancellationToken cancellationToken = default)
    {
        tipo = tipo.Trim().ToUpperInvariant();

        if (tipo is not ("S" or "I"))
        {
            throw new ArgumentException(
                "El tipo debe ser S (Socios) o I (Instituciones).");
        }

        var ganador = await _repository.ObtenerGanadorAsync(
            tipo,
            cancellationToken);

        if (ganador is null)
        {
            return null;
        }

        return new SorteoNumeroResponse(
            ganador.Id,
            ganador.Numero,
            ganador.Tipo,
            ganador.Estado
        );
    }
}