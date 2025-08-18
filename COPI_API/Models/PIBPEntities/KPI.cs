using System;
using System.Collections.Generic;
namespace COPI_API.Models.PIBPEntities
{
    public class KPI
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public Nivel? Nivel { get; set; }
        public Eixo? Eixo { get; set; }
        public double? Pontuacao { get; set; }
    }
}
