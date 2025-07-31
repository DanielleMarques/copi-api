namespace COPI_API.Models
{
    public class AcaoEstrategica
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public int MetaId { get; set; }
        public string? AcaoExecutada { get; set; }
        public string? ResponsavelExecucao { get; set; }
        public string? ResponsavelAprovacao { get; set; }
        public int Progresso { get; set; } // 0 a 100
        public string? Comentarios { get; set; }
        public string? DocumentosAnalise { get; set; }
        public string? Evidencia { get; set; }
        public bool DocumentoValidado { get; set; }
        public DateTime? PrazoFinal { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public bool Concluido { get; set; }
        public string? Status { get; set; }
        public bool Batida { get; set; } // NOVO: indica se a ação foi batida

        // Relação: AcaoEstrategica pertence a uma Meta
        public Meta? Meta { get; set; }
        // Relação: AcaoEstrategica tem várias Tarefas
        public ICollection<Tarefa>? Tarefas { get; set; }
    }
}