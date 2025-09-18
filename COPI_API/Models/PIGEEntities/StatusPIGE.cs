using COPI_API.Models.PIBPEntities;

namespace COPI_API.Models.PIGEEntities
{
    public class StatusPIGE
    {
        public int Id { get; set; }
        public int CicloPIGEId { get; set; }
        public CicloPIGE? CicloPIGE { get; set; }
        public StatusProcessoUnidadeKPIpige Status { get; set; } = StatusProcessoUnidadeKPIpige.EmAnalise;
        public int UnidadeKPIPIGEId { get; set; }
        public UnidadeKPIPIGE? UnidadeKPIPIGE { get; set; }
    }
    public enum StatusProcessoUnidadeKPIpige
    {
        EmAnalise,
        ParaRevisao,
        Aprovado
    }
}
