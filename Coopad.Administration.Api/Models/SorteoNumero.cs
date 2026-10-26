
namespace Coopad.Administration.Api.Models;

public sealed class SorteoNumero
{
    public int Id { get; set; }

    public int Numero { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public bool Estado { get; set; }
}
