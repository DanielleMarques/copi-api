using System;

namespace COPI_API.Models.MetaEntities
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public int Progresso { get; set; }
        public string? Status { get; set; }
        public string? StatusExecucao { get; set; }
        public int AcaoEstrategicaId { get; set; }
        public string? Responsavel { get; set; }
        public string? Comentario { get; set; }
        public string? AvaliacaoDoc { get; set; }
        public bool Batida { get; set; }
        public DateTime? PrazoFinal { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public AcaoEstrategica? AcaoEstrategica { get; set; }
    }
}
