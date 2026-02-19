using COPI_API.Models.AdminEntities;

namespace COPI_API.Models.DPEEntities
{
    public class Documento
    {
        public enum TipoDocumento
        {
            Manifestacao = 1,
            DeclaracaoMotivacao = 2,
            Anonimizado = 3
        }

        public Guid Id { get; set; }
        public string? NomeOriginal { get; set; }
        public string? NomeArmazenado { get; set; }
        public TipoDocumento Tipo { get; set; } 
        public string? CaminhoArquivo { get; set; }
        public string? TextoExtraido { get; set; } // OCR / PDF Reader
        public bool Anonimizado { get; set; }
        public string ContentType { get; set; } = null!;
        public long Tamanho { get; set; }
        public DateTimeOffset CriadoEm { get; set; }
        public string? CriadoPor { get; set; }
        public int AfastamentoId { get; set; }
        public Afastamento? Afastamento { get; set; }
        public int DivisaoId { get; set; }
        public Divisao? Divisao { get; set; }
    }
}
