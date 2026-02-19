namespace COPI_API.Models.DPEEntities
{
    public class Ementario
    {
        public int Id { get; set; }
        public int AfastamentoId { get; set; }
        public Afastamento? Afastamento { get; set; }
        public string? EmentaResumo { get; set; }
        public string? NumeroSei { get; set; }
        public string? Fundamentacao { get; set; }
        public DateTimeOffset CriadoEm { get; set; }
    }
}
