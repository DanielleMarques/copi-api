namespace COPI_API.Models
{
    public class Servidor
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public int DivisaoId { get; set; }

        public string? Celular { get; set; }
        public string? Status { get; set; }
        public string? CargoOuFuncao { get; set; }
        public string? PontoSei { get; set; }
        public string? ChefiaImediata { get; set; }
        public string? RF { get; set; }
        public string? HorarioEntrada { get; set; }
        public string? HorarioSaida { get; set; }

        public Divisao? Divisao { get; set; }
        public ICollection<UnidadeKPI>? UnidadesKpiResponsaveis { get; set; }

    }
}
