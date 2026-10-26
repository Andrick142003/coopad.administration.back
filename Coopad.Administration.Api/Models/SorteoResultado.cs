namespace Coopad.Administration.Api.Models
{
    public class SorteoResultado
    {
        public long Id { get; set; }

        public int NumeroId { get; set; }

        public int Numero { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public DateTime FechaSorteo { get; set; }
    }
}
