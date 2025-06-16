namespace COPI_API.Models
{
    public class Eixo
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public decimal Peso { get; set; }  // NOVO CAMPO

        public ICollection<KPI>? KPIs { get; set; }
    }
}