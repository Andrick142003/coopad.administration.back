namespace Coopad.Administration.Api.Models
{
    public class FechasRango
    {
        public int? Id { get; set; }
        public int? Anio { get; set; }
        public int? Mes { get; set; }
        public int? Semana { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
