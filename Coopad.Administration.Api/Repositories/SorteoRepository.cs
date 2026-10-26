using Coopad.Administration.Api.Data;
using Coopad.Administration.Api.Models;
using Coopad.Administration.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Coopad.Administration.Api.Repositories;

public sealed class SorteoRepository : ISorteoRepository
{
    private readonly SecurityDbContext _context;

    public SorteoRepository(SecurityDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SorteoNumero>> ObtenerPorTipoAsync(
        string tipo,
        CancellationToken cancellationToken = default)
    {
        return await _context.SorteoNumeros
            .AsNoTracking()
            .Where(x => x.Tipo == tipo)
            .OrderBy(x => x.Numero)
            .ToListAsync(cancellationToken);
    }

    public async Task<SorteoNumero?> ObtenerPorIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.SorteoNumeros
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task ActualizarEstadoAsync(
        SorteoNumero numero,
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<SorteoNumero?> ObtenerGanadorAsync(
    string tipo,
    CancellationToken cancellationToken = default)
    {
        var candidatos = await _context.SorteoNumeros
            .AsNoTracking()
            .Where(x => x.Tipo == tipo && x.Estado)
            .ToListAsync(cancellationToken);

        if (candidatos.Count == 0)
        {
            return null;
        }

        var indice = RandomNumberGenerator.GetInt32(candidatos.Count);

        return candidatos[indice];
    }
}