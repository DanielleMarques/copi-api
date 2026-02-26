namespace COPI_API.Models.DPEEntities
{
    public class TextosDeclaracao
    {
        public int Id { get; set; }
        public string? NomeCompleto { get; set; }
        public string? SituacaoFuncional { get; set; }
        public string? RF { get; set; }
        public string? Cargo { get; set; }
        public string? OrgaoEntidade { get; set; }
        public string? TipoEvento { get; set; }
        public string? NomeEvento { get; set; }
        public string? TipoParticipacao { get; set; }
        public string? Organizadores { get; set; }
        public string? Patrocinadores { get; set; }
        public string? DataInicioFim { get; set; }
        public string? DataIdaVolta { get; set; }
        public string? Cidade { get; set; }
        public string? DescricaoEvento { get; set; }
        public string? MotivacaoEvento { get; set; }
        public Guid DocumentoId { get; set; }
        public Documento? Documentos { get; set; }
        public int EmentarioId { get; set; }
        public Ementario? Ementarios { get; set; }
    }
}
