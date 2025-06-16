namespace COPI_API.Models
{
    public class Nivel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public decimal Valor { get; set; }

        public ICollection<KPI>? KPIs { get; set; }
    }

}
