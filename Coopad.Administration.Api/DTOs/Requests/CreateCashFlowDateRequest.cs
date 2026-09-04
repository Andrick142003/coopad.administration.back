namespace Coopad.Administration.Api.DTOs.Requests
{
    public class CreateCashFlowDateRequest
    {
        public int? Id { get; set; }
        public int Anio { get; set; }

        public int Mes { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int Semana { get; set; }
    }
}
