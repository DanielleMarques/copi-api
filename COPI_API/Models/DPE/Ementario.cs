namespace COPI_API.Models.DPE
{
    public class Ementario
    {
        public int EmentarioId { get; set; }
        public string? NumeroEmentario { get; set; }
        public string? TipoDeAfastamento { get; set; }
        public string? RazaoDoAfastamento { get; set; }
        public string? ManifestacaoCGM { get; set; }
        public DateTime? DataEnvioAfastamento { get; set; }
        public string? StatusDoAfastamento { get; set; }
        public DateTime? DataInicioAfastamento { get; set; }
        public DateTime? DataFimAfastamento { get; set; }
        public int? TempoDeAfastamentoDias { get; set; }
        public double? PrazoEnvioViagemXSolicitacao { get; set; }
        public string? InferiorA10DiasJustificativa { get; set; }
        public string? OrgaoEntidade { get; set; }
        //Utilizar do sistema de unidades da COPI
        public string? Sigla { get; set; }
        public string? Categoria { get; set; }
        public string? Subcategoria { get; set; }
        public string? Vencimento { get; set; }
        public string? FormaDeCusteio { get; set; }
        public string? TipoDeViagem { get; set; }
        public string? IndicioConflitoDeInteresses { get; set; }
        public string? Ementas { get; set; }
        public string? Observacoes { get; set; }
    }
}
