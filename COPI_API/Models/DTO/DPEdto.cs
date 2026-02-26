using COPI_API.Models.DPEEntities;
using static COPI_API.Models.DPEEntities.Afastamento;
using static COPI_API.Models.DPEEntities.Documento;

namespace COPI_API.Models.DTO
{
    public class DPEdto
    {
        public class AfastamentoCreateDto
        {
            public string? NumeroProcesso { get; set; }
            public TipoAfastamento Tipo { get; set; }

            public DateTime DataInicio { get; set; }
            public DateTime DataFim { get; set; }
            public string? ServidorMatricula { get; set; }
            public string? ServidorCargo { get; set; }
        }

        public class AfastamentoResponseDto
        {
            public int Id { get; set; }
            public string? NumeroProcesso { get; set; }
            public TipoAfastamento Tipo { get; set; }
            public StatusAfastamento Status { get; set; }
            public bool PossuiInconsistencias { get; set; }
        }

        public class UploadDocumentoDto
        {
            public IFormFile? Arquivo { get; set; }
            public TipoDocumento Tipo { get; set; }
            public int DivisaoId { get; set; }
            public TipoAfastamento TipoAfastamento { get; set; }
        }

        public class DocumentoProcessadoDto
        {
            public string? Ementa { get; set; }
            public string? NumeroProcesso { get; set; }
        }

        public class DocumentoAgrupadoDto
        {
            public int Mes { get; set; }
            public string TipoAfastamento { get; set; } = null!;
            public List<DocumentoResponseDto> Documentos { get; set; } = new();
        }

        public class DocumentoResponseDto
        {
            public Guid Id { get; set; }
            public int AfastamentoId { get; set; }
            public string? NomeOriginal { get; set; }
            public Documento.TipoDocumento? Tipo { get; set; }
            public long Tamanho { get; set; }
            public DateTimeOffset CriadoEm { get; set; }
            public string? CriadoPor { get; set; }
            public string? NumeroSei { get; set; }
            public DadosDeclaracaoDto? DadosDeclaracao { get; set; }
            public DateTime? DataAssinatura { get; set; }
        }

        public class DadosDeclaracaoDto
        {
            public string? NomeCompleto { get; set; }
            public string? SituacaoFuncional { get; set; }
            public string? RF { get; set; }
            public string? Cargo { get; set; }
            public string? Orgao { get; set; }
            public string? TipoEvento { get; set; }
            public string? NomeEvento { get; set; }
            public string? DataInicioFim { get; set; }
            public string? DataIdaVolta { get; set; }
            public string? DescricaoEvento { get; set; }
            public string? Cidade { get; set; }
            public string? Pais { get; set; }
            public string? TipoParticipação { get; set; }
            public string? Organizador { get; set; }
            public string? Patrocinador { get; set; }
            public string? MotivacaoEvento { get; set; }
        }

        public class ComparacaoResultadoDto
        {
            public bool PossuiInconsistencias { get; set; }
            public List<string>? CamposDivergentes { get; set; }
        }

        public class RelatorioFiltroDto
        {
            public DateTime? DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
            public TipoAfastamento Tipo { get; set; }
            public StatusAfastamento Status { get; set; }
        }

    }
}
