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
        }

        public class DocumentoResponseDto
        {
            public Guid Id { get; set; }
            public int AfastamentoId { get; set; }
            public string? NomeOriginal { get; set; }
            public Documento.TipoDocumento? Tipo { get; set; }
            public long Tamanho { get; set; }
            public DateTime CriadoEm { get; set; }
            public string? CriadoPor { get; set; }
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
