using COPI_API.Models.PIGEEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace COPI_API.Models.PIGEEntities
{
    public class AvaliacaoServicePIGE
    {
        private readonly AppDbContext _context;

        public AvaliacaoServicePIGE(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object?> CalcularIMPIGE(int unidadeKpiId, int cicloId, bool salvar = false)
        {
            var ciclo = await _context.Set<CicloPIGE>().FirstOrDefaultAsync(c => c.Id == cicloId);
            if (ciclo == null) return null;

            var niveis = await _context.Set<NivelPIGE>().OrderBy(n => n.Valor).ToListAsync();
            var eixos = await _context.Set<EixoPIGE>().ToListAsync();

            var resultados = await _context.Set<ResultadoKPIPIGE>()
                .Include(r => r.KPIPIGE)
                    .ThenInclude(k => k.Nivel)
                .Include(r => r.KPIPIGE)
                    .ThenInclude(k => k.Eixo)
                .Where(r => r.UnidadeKPIPIGEId == unidadeKpiId && r.CicloPIGEId == cicloId)
                .ToListAsync();

            if (!resultados.Any()) return null;

            var notasPorNivel = new Dictionary<string, decimal>();
            var atingiuNivel = new Dictionary<string, bool>();

            foreach (var nivel in niveis)
            {
                var kpisDoNivel = resultados
                    .Where(r => r.KPIPIGE?.Nivel?.Id == nivel.Id)
                    .ToList();

                decimal notaNivel = 0m;

                foreach (var eixo in eixos)
                {
                    var resultadosPorEixoENivel = kpisDoNivel
                        .Where(r => r.KPIPIGE?.Eixo?.Id == eixo.Id)
                        .ToList();

                    decimal somaPontuacao = resultadosPorEixoENivel
                        .Where(r => r.Status == "SIM" || r.Status == "NAO APLICAVEL")
                        .Sum(r => (decimal)(r.KPIPIGE?.Pontuacao ?? 0));

                    notaNivel += somaPontuacao * eixo.Peso;
                }

                notaNivel *= (nivel.Valor);
                notasPorNivel[nivel.Nome!] = notaNivel;
                atingiuNivel[nivel.Nome!] = kpisDoNivel
                    .All(r => r.Status == "SIM" || r.Status == "NAO APLICAVEL");
            }

            decimal notaIMPIGE = niveis
                .Sum(n =>
                {
                    var nota = notasPorNivel.GetValueOrDefault(n.Nome!, 0m);
                    return n.Valor * (nota / (n.Valor));
                });

            if (salvar)
            {
                var avaliacao = new AvaliacaoPIGE
                {
                    NotaIMPIGE = notaIMPIGE,
                    Resultados = resultados
                };
                _context.Set<AvaliacaoPIGE>().Add(avaliacao);
                await _context.SaveChangesAsync();
            }

            var unidade = await _context.Set<UnidadeKPIPIGE>()
                .Include(uk => uk.Unidade)
                .FirstOrDefaultAsync(uk => uk.Id == unidadeKpiId);
            string? nomeUnidade = unidade?.Unidade?.Nome;
            string? nomeCiclo = ciclo.Nome;
            var resultadosDetalhados = resultados
                .Select(r => new
                {
                    NomeKPI = r.KPIPIGE?.Nome ?? "KPI Desconhecido",
                    Status = r.Status,
                    Pontuacao = (r.Status == "SIM" || r.Status == "NAO APLICAVEL")
                        ? ((decimal)(r.KPIPIGE?.Pontuacao ?? 0) * (r.KPIPIGE?.Eixo?.Peso ?? 1) * (r.KPIPIGE?.Nivel?.Valor ?? 1))
                        : 0,
                    Prova = r.Prova,
                    AvaliacaoEscrita = r.AvaliacaoEscrita,
                    Eixo = r.KPIPIGE?.Eixo?.Nome ?? "Desconhecido"
                }).ToList();

            return new
            {
                NotaIMPIGE = Math.Round(notaIMPIGE, 2),
                NotasPorNivel = notasPorNivel.ToDictionary(kvp => kvp.Key, kvp => Math.Round(kvp.Value, 2)),
                NiveisAtingidos = atingiuNivel,
                NomeUnidade = nomeUnidade,
                NomeCiclo = nomeCiclo,
                ResultadosDetalhados = resultadosDetalhados
            };
        }

        public async Task<object> ObterResultadosPorEixo(int cicloId)
        {
            var unidadesKpi = await GetUnidadesKPIPIGEComResultadosDoCiclo(cicloId);
            if (!unidadesKpi.Any())
                return new { mediaPorEixo = new Dictionary<string, decimal>(), resultados = new List<Dictionary<string, object>>() };

            var resultadosUnidade = new List<Dictionary<string, object>>();
            var somaEixosGeral = new Dictionary<string, decimal>();
            var contagemUnidadesPorEixo = new Dictionary<string, int>();
            var todosEixos = new HashSet<string>();

            foreach (var unidadeKpi in unidadesKpi)
            {
                var resultado = await CalcularIMPIGE(unidadeKpi.Id, cicloId);
                if (resultado == null) continue;

                var linha = new Dictionary<string, object>
                {
                    { "Unidade", ((dynamic)resultado).NomeUnidade }
                };
                // Corrigindo acesso dinâmico para evitar erro CS1977
                var resultadosDetalhados = ((dynamic)resultado).ResultadosDetalhados as IEnumerable<object>;
                var somaPorEixo = new Dictionary<string, decimal>();
                if (resultadosDetalhados != null)
                {
                    foreach (var item in resultadosDetalhados)
                    {
                        var eixo = (string)item.GetType().GetProperty("Eixo")?.GetValue(item) ?? "Desconhecido";
                        var pontuacaoObj = item.GetType().GetProperty("Pontuacao")?.GetValue(item);
                        var pontuacao = pontuacaoObj is decimal d ? d : 0m;
                        if (!somaPorEixo.ContainsKey(eixo))
                            somaPorEixo[eixo] = 0m;
                        somaPorEixo[eixo] += pontuacao;
                    }
                }

                foreach (var eixo in somaPorEixo)
                {
                    var eixoNome = eixo.Key;
                    var pontuacaoTotal = Math.Round(eixo.Value, 2);

                    linha[eixoNome] = pontuacaoTotal;
                    todosEixos.Add(eixoNome);

                    if (!somaEixosGeral.ContainsKey(eixoNome))
                        somaEixosGeral[eixoNome] = 0;
                    somaEixosGeral[eixoNome] += pontuacaoTotal;

                    if (!contagemUnidadesPorEixo.ContainsKey(eixoNome))
                        contagemUnidadesPorEixo[eixoNome] = 0;
                    contagemUnidadesPorEixo[eixoNome]++;
                }

                resultadosUnidade.Add(linha);
            }

            foreach (var linha in resultadosUnidade)
            {
                foreach (var eixo in todosEixos)
                {
                    if (!linha.ContainsKey(eixo))
                        linha[eixo] = 0m;
                }
            }

            var mediaPorEixo = todosEixos.ToDictionary(
                eixo => eixo,
                eixo =>
                {
                    var soma = somaEixosGeral.ContainsKey(eixo) ? somaEixosGeral[eixo] : 0;
                    var count = contagemUnidadesPorEixo.ContainsKey(eixo) ? contagemUnidadesPorEixo[eixo] : 1;
                    return Math.Round(soma / count, 2);
                }
            );

            return new
            {
                mediaPorEixo,
                resultados = resultadosUnidade
            };
        }

        public async Task<List<UnidadeKPIPIGE>> GetUnidadesKPIPIGEComResultadosDoCiclo(int cicloId)
        {
            return await _context.Set<ResultadoKPIPIGE>()
                .Where(r => r.CicloPIGEId == cicloId)
                .Select(r => r.UnidadeKPIPIGE)
                .Distinct()
                .Include(uk => uk.Unidade)
                .ToListAsync();
        }
    }
}
