using Microsoft.AspNetCore.Mvc;

namespace COPI_API.Models.DPEEntities
{
    public class Afastamento
    {
        public enum TipoAfastamento
        {
            InteresseInst,
            InteressePessoal,
        }

        public enum StatusAfastamento
        {
            EmAnalise,
            Aprovado,
            Indeferido
        }

        public int Id { get; set; }
        public string? NumeroProcesso { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public TipoAfastamento Tipo { get; set; }
        public string? ServidorNome { get; set; }
        public string? ServidorMatricula { get; set; }
        public string? ServidorCargo { get; set; }
        public StatusAfastamento Status { get; set; }
        public int DeclaracaoPdfId { get; set; }
        public int ManifestacaoPdfId { get; set; }
        public bool PossuiInconsistencia { get; set; }
        public Ementario? Ementario { get; set; }
        public DateTimeOffset CriadoEm { get; set; }
        public string? CriadoPor { get; set; }
    }

}
