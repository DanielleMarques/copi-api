using COPI_API.Models;
using System.Text.Json.Serialization;

public class KPI
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public Nivel? Nivel { get; set; }
    public Eixo? Eixo { get; set; }
    public double? Pontuacao { get; set; }
}

