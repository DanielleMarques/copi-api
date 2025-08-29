namespace COPI_API.Models.ITAEntities
{
    public class CicloAvaliacaoITA
    {
        public int CicloAvaliacaoITAId { get; set; }
        public int Ano { get; set; }
        public int Semestre { get; set; } // 1 ou 2
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        // Relacionamento: ciclo possui várias avaliações
        public ICollection<AvaliacaoITA> AvaliacoesITA { get; set; } = new List<AvaliacaoITA>();
    }
}
