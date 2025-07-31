namespace COPI_API.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public int Progresso { get; set; } // 0 a 100
        public string? Status { get; set; } // "Pendente", "Em andamento", "Concluída"
        public string? StatusExecucao { get; set; } // "Não iniciado", "Em andamento", "Parado", "Concluído"
        public int AcaoEstrategicaId { get; set; } // FK obrigatória para AcaoEstrategica
        public string? Responsavel { get; set; }
        public string? Comentario { get; set; }
        public string? AvaliacaoDoc { get; set; }
        public bool Batida { get; set; }
        public DateTime? PrazoFinal { get; set; }
        public DateTime? DataCumprimento { get; set; }

        // Relação: Tarefa pertence a uma AcaoEstrategica
        public AcaoEstrategica? AcaoEstrategica { get; set; }
    }
}