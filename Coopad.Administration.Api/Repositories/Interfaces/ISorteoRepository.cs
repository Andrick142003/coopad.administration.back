using Coopad.Administration.Api.Models;

namespace Coopad.Administration.Api.Repositories.Interfaces
{
    public interface ISorteoRepository
    {
        Task<IReadOnlyList<SorteoNumero>> ObtenerPorTipoAsync(
    string tipo,
    CancellationToken cancellationToken = default);

        Task<SorteoNumero?> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task ActualizarEstadoAsync(
            SorteoNumero numero,
            CancellationToken cancellationToken = default);


        Task<SorteoNumero?> ObtenerGanadorAsync(
    string tipo,
    CancellationToken cancellationToken = default);
    }



}
