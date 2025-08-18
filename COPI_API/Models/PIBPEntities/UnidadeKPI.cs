using COPI_API.Models.PIBPEntities;
using COPI_API.Models.AdminEntities;
using System;
using System.Collections.Generic;

namespace COPI_API.Models.PIBPEntities
{
    public class UnidadeKPI
    {
        public int Id { get; set; }
        public int UnidadeId { get; set; }
        public Unidade? Unidade { get; set; }
        public int ServidorId { get; set; }
        public Servidor? Servidor { get; set; }
        public string? SEI { get; set; }
        public List<ResultadoKPI>? Resultados { get; set; }
    }
}
