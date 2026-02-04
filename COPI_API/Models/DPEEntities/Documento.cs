namespace COPI_API.Models.DPEEntities
{
    public class Documento
    {
        public enum TipoDocumento
        {
            DeclaracaoMotivacao = 1,
            Manifestacao = 2,
            Anonimizado = 3
        }

        public Guid Id { get; set; }
        public int AfastamentoId { get; set; }
        public string? NomeOriginal { get; set; }
        public string? NomeArmazenado { get; set; }
        public TipoDocumento Tipo { get; set; } 
        public string? CaminhoArquivo { get; set; }
        public string? TextoExtraido { get; set; } // OCR / PDF Reader
        public bool Anonimizado { get; set; }
        public string? ContentType { get; set; }
        public long Tamanho { get; set; }
        public DateTime CriadoEm { get; set; }
        public string? CriadoPor { get; set; }
    }
}
