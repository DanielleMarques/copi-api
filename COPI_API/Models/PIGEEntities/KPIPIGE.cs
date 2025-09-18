namespace COPI_API.Models.PIGEEntities
{
    public class KPIPIGE
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public NivelPIGE? Nivel { get; set; }
        public EixoPIGE? Eixo { get; set; }
        public double? Pontuacao { get; set; }
    }
}
