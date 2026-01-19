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
        public string? AvaliacaoDi { get; set; }
        public string? Entregavel { get; set; }
        public string? Objetivo { get; set; }
        public string? Premissas { get; set; }
        public string? Restricoes { get; set; }
        public string? Riscos { get; set; }
        public string? Responsavel { get; set; }
        public string? Aprovador { get; set; }
        public string? Consultado { get; set; }
        public string? Informado { get; set; }
        public int DivisaoId { get; set; }
        public string? OrigemMeta { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public DateTime DataInic { get; set; }
        public DateTime DataFim { get; set; }
    }
    public class MetaOutputDTO
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Status { get; set; }
        public string? Tipo { get; set; }
        public string? Setor { get; set; }
        public string? AvaliacaoDi { get; set; }
        public string? Entregavel { get; set; }
        public string? Objetivo { get; set; }
        public string? Premissas { get; set; }
        public string? Restricoes { get; set; }
        public string? Riscos { get; set; }
        public string? Responsavel { get; set; }
        public string? Aprovador { get; set; }
        public string? Consultado { get; set; }
        public string? Informado { get; set; }
        public int DivisaoId { get; set; }
        public string? OrigemMeta { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public DateTime DataInic { get; set; }
        public DateTime DataFim { get; set; }
        public int Progresso { get; set; }
        public List<AcaoEstrategicaOutputDTO>? AcoesEstrategicas { get; set; }
    }
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
    public class TarefaInputDTO
    {
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
    }
    public class TarefaOutputDTO
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
        public string? NomeMeta { get; set; }
    }
}
