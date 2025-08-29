using COPI_API.Models.AdminEntities;

namespace COPI_API.Models.ITAEntities
{
    public enum VersaoITA
    {
        ProgramaDeMetas, // PdM
        Ampliada         // Inclui indireta
    }

    public class AvaliacaoITA
    {
        public int AvaliacaoITAId { get; set; }

        public int UnidadeId { get; set; }
        public Unidade Unidade { get; set; }

        public int CicloAvaliacaoITAId { get; set; }
        public CicloAvaliacaoITA CicloAvaliacaoITA { get; set; }

        // Notas consolidadas
        public double NotaBA { get; set; }
        public double NotaBP { get; set; }
        public double NotaBQ { get; set; }
        public double NotaTP { get; set; }
        public double NotaDA { get; set; }

        public double ResultadoITA { get; set; }

        // Nova informação
        public VersaoITA Versao { get; set; }

        public ICollection<ItemAvaliacaoITA> Itens { get; set; } = new List<ItemAvaliacaoITA>();
    }

}
