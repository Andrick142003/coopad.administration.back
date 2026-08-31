namespace Coopad.Administration.Api.DTOs.Responses
{
    public class CashFlowValues
    {
        public int codigo { get; set; }
        public string descripcion { get; set; } = null!;
        public decimal proyectado { get; set; }
        public decimal valor { get; set; }
        public decimal variacion { get; set; }
        public decimal variacionPorcentual { get; set; }
    }
}
