using Coopad.Administration.Api.DTOs.Responses;

namespace Coopad.Administration.Api.Services.Interfaces
{
    public interface ISorteoService
    {
        Task<IReadOnlyList<SorteoNumeroResponse>> ObtenerNumerosAsync(
    string tipo,
    CancellationToken cancellationToken = default);

        Task<SorteoNumeroResponse?> ActualizarEstadoAsync(
            int id,
            bool estado,
            CancellationToken cancellationToken = default);


        Task<SorteoNumeroResponse?> RealizarSorteoAsync(
    string tipo,
    CancellationToken cancellationToken = default);
    }
}
