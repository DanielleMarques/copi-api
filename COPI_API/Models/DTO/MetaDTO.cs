using System;
using System.Collections.Generic;

namespace COPI_API.Models.DTO
{
    // DTO para entrada de Meta
    public class MetaInputDTO
    {
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Status { get; set; }
        public string? Tipo { get; set; }
        public string? Setor { get; set; }
        public int DivisaoId { get; set; }
        public string? OrigemMeta { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public DateTime DataInic { get; set; }
        public DateTime DataFim { get; set; }
    }

    // DTO para saída de Meta
    public class MetaOutputDTO
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Status { get; set; }
        public string? Tipo { get; set; }
        public string? Setor { get; set; }
        public int DivisaoId { get; set; }
        public string? OrigemMeta { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public DateTime DataInic { get; set; }
        public DateTime DataFim { get; set; }
        public int Progresso { get; set; }
        public List<AcaoEstrategicaOutputDTO>? AcoesEstrategicas { get; set; }
    }

    // DTO para entrada de Tarefa
    public class TarefaInputDTO
    {
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Status { get; set; }
        public string? StatusExecucao { get; set; }
        public int AcaoEstrategicaId { get; set; }
        public string? Responsavel { get; set; }
        public string? Comentario { get; set; }
        public string? AvaliacaoDoc { get; set; }
        public bool Batida { get; set; }
        public DateTime? PrazoFinal { get; set; }
        public DateTime? DataCumprimento { get; set; }
    }

    // DTO para saída de Tarefa
    public class TarefaOutputDTO
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Status { get; set; }
        public string? StatusExecucao { get; set; }
        public string? Responsavel { get; set; }
        public string? Comentario { get; set; }
        public string? AvaliacaoDoc { get; set; }
        public bool Batida { get; set; }
        public DateTime? PrazoFinal { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public int Progresso { get; set; }
        public int AcaoEstrategicaId { get; set; }
        public string? NomeMeta { get; internal set; }
    }

    // DTO para entrada de AcaoEstrategica
    public class AcaoEstrategicaInputDTO
    {
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public int MetaId { get; set; }
        public string? AcaoExecutada { get; set; }
        public string? ResponsavelExecucao { get; set; }
        public string? ResponsavelAprovacao { get; set; }
        public int Progresso { get; set; }
        public string? Comentarios { get; set; }
        public string? DocumentosAnalise { get; set; }
        public string? Evidencia { get; set; }
        public bool DocumentoValidado { get; set; }
        public DateTime? PrazoFinal { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public bool Concluido { get; set; }
        public string? Status { get; set; }
        public bool Batida { get; set; }
    }

    // DTO para saída de AcaoEstrategica
    public class AcaoEstrategicaOutputDTO
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public int MetaId { get; set; }
        public string? AcaoExecutada { get; set; }
        public string? ResponsavelExecucao { get; set; }
        public string? ResponsavelAprovacao { get; set; }
        public int Progresso { get; set; }
        public string? Comentarios { get; set; }
        public string? DocumentosAnalise { get; set; }
        public string? Evidencia { get; set; }
        public bool DocumentoValidado { get; set; }
        public DateTime? PrazoFinal { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public bool Concluido { get; set; }
        public string? Status { get; set; }
        public bool Batida { get; set; }
        public List<TarefaOutputDTO>? Tarefas { get; set; }
    }
}
