using System;
using System.Collections.Generic;
namespace COPI_API.Models.PIBPEntities
{
    public class Ciclo
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public bool? Encerrado { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}
