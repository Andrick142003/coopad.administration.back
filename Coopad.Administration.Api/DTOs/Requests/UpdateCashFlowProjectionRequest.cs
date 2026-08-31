namespace Coopad.Administration.Api.DTOs.Requests
{
    public class UpdateCashFlowProjectionRequest
    {
        public int Anio { get; set; }

        public int Mes { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int Semana { get; set; }

        public string TipoSaldo { get; set; } = string.Empty;

        public string Tipo { get; set; } = string.Empty;

        public decimal Proyeccion { get; set; }
    }
}
