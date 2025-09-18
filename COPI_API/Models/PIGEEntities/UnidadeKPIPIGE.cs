using COPI_API.Models.AdminEntities;
using System.Collections.Generic;

namespace COPI_API.Models.PIGEEntities
{
    public class UnidadeKPIPIGE
    {
        public int Id { get; set; }
        public int UnidadeId { get; set; }
        public Unidade? Unidade { get; set; }
        public int ServidorId { get; set; }
        public Servidor? Servidor { get; set; }
        public string? SEI { get; set; }
        public List<ResultadoKPIPIGE>? Resultados { get; set; }
    }
}
