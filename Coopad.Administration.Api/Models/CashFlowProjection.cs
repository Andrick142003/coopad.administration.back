namespace Coopad.Administration.Api.Models
{
    public class CashFlowProjection 
    { 
        public int? Id { get; set; } 
        public int? Anio { get; set; } 
        public int? Mes { get; set; } 
        public int? Semana { get; set; }
        public string? TipoSaldo { get; set; } = string.Empty; 
        public string? Tipo { get; set; } = string.Empty;
        public decimal? Proyeccion { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; } }
}
