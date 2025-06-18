using COPI_API.DTOs;
using COPI_API.Models;
using Microsoft.EntityFrameworkCore;

namespace COPI_API.Services
{
    public class AvaliacaoService
    {
        private readonly AppDbContext _context;

        public AvaliacaoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultadoAvaliacaoDTO?> CalcularIMPIBP(int unidadeKpiId, int cicloId, bool salvar = false)
        {
            // Busca o ciclo correspondente
            var ciclo = await _context.Ciclos.FirstOrDefaultAsync(c => c.Id == cicloId);
            if (ciclo == null) return null;

            // Carrega níveis (em ordem de valor) e eixos
            var niveis = await _context.Niveis.OrderBy(n => n.Valor).ToListAsync();
            var eixos = await _context.Eixos.ToListAsync();

            // Busca os resultados KPI da unidade e ciclo informados
            var resultados = await _context.ResultadosKPI
                .Include(r => r.KPI)
                    .ThenInclude(k => k.Nivel)
                .Include(r => r.KPI)
                    .ThenInclude(k => k.Eixo)
                .Where(r => r.UnidadeKPIId == unidadeKpiId && r.CicloId == cicloId)
                .ToListAsync();

            if (!resultados.Any()) return null;

            var notasPorNivel = new Dictionary<string, decimal>();
            var atingiuNivel = new Dictionary<string, bool>();

            foreach (var nivel in niveis)
            {
                // Filtra KPIs deste nível
                var kpisDoNivel = resultados
                    .Where(r => r.KPI?.Nivel?.Id == nivel.Id)
                    .ToList();

                decimal notaNivel = 0m;

                foreach (var eixo in eixos)
                {
                    // Filtra KPIs do nível e eixo atual
                    var resultadosPorEixoENivel = kpisDoNivel
                        .Where(r => r.KPI?.Eixo?.Id == eixo.Id)
                        .ToList();

                    // Soma pontuação dos KPIs com status SIM ou NAO_APLICAVEL
                    decimal somaPontuacao = resultadosPorEixoENivel
                        .Where(r => r.Status == "SIM" || r.Status == "NAO APLICAVEL")
                        .Sum(r => (decimal)(r.KPI?.Pontuacao ?? 0));

                    // Aplica o peso do eixo
                    notaNivel += somaPontuacao * eixo.Peso;
                }

                // Multiplica a nota calculada pelo valor do nível * 10
                // Exemplo: se o nível tem valor 0.4, a nota máxima dele será 4
                notaNivel *= (nivel.Valor);

                notasPorNivel[nivel.Nome!] = notaNivel;

                // Verifica se todos os KPIs do nível foram atingidos (SIM ou NAO_APLICAVEL)
                atingiuNivel[nivel.Nome!] = kpisDoNivel
                    .All(r => r.Status == "SIM" || r.Status == "NAO APLICAVEL");
            }

            // Cálculo da nota total ponderada (IM-PIBP)
            decimal notaIMPIBP = niveis
                .Sum(n =>
                {
                    var nota = notasPorNivel.GetValueOrDefault(n.Nome!, 0m);
                    // Como nota por nível foi multiplicada por (n.Valor * 10), aqui dividimos por 10 para manter o cálculo da nota final correto
                    return n.Valor * (nota / (n.Valor));
                });

            if (salvar)
            {
                var avaliacao = new Avaliacao
                {
                    NotaIMPIBP = notaIMPIBP,
                    Resultados = resultados
                };

                _context.Avaliacoes.Add(avaliacao);
                await _context.SaveChangesAsync();
            }

            var unidade = await _context.UnidadesKPI
                .Include(uk => uk.Unidade)
                .FirstOrDefaultAsync(uk => uk.Id == unidadeKpiId);
            string? nomeUnidade = unidade?.Unidade?.Nome;
            string? nomeCiclo = ciclo.Nome;
            // Adiciona os resultados individuais (nome do KPI e status)
            var resultadosDetalhados = resultados
            .Select(r => new KpiResultadoDTO
            {
                NomeKPI = r.KPI?.Nome ?? "KPI Desconhecido",
                Status = r.Status,
                Pontuacao = (r.Status == "SIM" || r.Status == "NAO APLICAVEL")
                    ? ((decimal)(r.KPI?.Pontuacao ?? 0) * (r.KPI?.Eixo?.Peso ?? 1) * (r.KPI?.Nivel?.Valor ?? 1))
                    : 0,
                Prova = r.Prova,
                AvaliacaoEscrita = r.AvaliacaoEscrita,
                Eixo = r.KPI?.Eixo?.Nome ?? "Desconhecido" 
            }).ToList();

            return new ResultadoAvaliacaoDTO
            {
                NotaIMPIBP = Math.Round(notaIMPIBP, 2),
                NotasPorNivel = notasPorNivel.ToDictionary(kvp => kvp.Key, kvp => Math.Round(kvp.Value, 2)),
                NiveisAtingidos = atingiuNivel,
                NomeUnidade = nomeUnidade,
                NomeCiclo = nomeCiclo,
                ResultadosDetalhados = resultadosDetalhados
            };
        }
        public async Task<object> ObterResultadosPorEixo(int cicloId)
        {
            var unidadesKpi = await GetUnidadesKPIComResultadosDoCiclo(cicloId);
            if (!unidadesKpi.Any())
                return new { mediaPorEixo = new Dictionary<string, decimal>(), resultados = new List<Dictionary<string, object>>() };

            var resultadosUnidade = new List<Dictionary<string, object>>();
            var somaEixosGeral = new Dictionary<string, decimal>();
            var contagemUnidadesPorEixo = new Dictionary<string, int>();
            var todosEixos = new HashSet<string>();

            foreach (var unidadeKpi in unidadesKpi)
            {
                var resultado = await CalcularIMPIBP(unidadeKpi.Id, cicloId);
                if (resultado == null) continue;

                var linha = new Dictionary<string, object>
        {
            { "Unidade", resultado.NomeUnidade }
        };
                //.Where(r => r.KPI?.Nivel?.Id == nivel.Id)
                var somaPorEixo = resultado.ResultadosDetalhados
                    .GroupBy(kpi => kpi.Eixo ?? "Desconhecido")
                    .ToDictionary(
                        g => g.Key,
                        g => g.Sum(k => k.Pontuacao)
                    );

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

            // Garante que todas as linhas tenham todos os eixos como chave (mesmo com 0)
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


        public async Task<List<UnidadeKPI>> GetUnidadesKPIComResultadosDoCiclo(int cicloId)
        {
            return await _context.ResultadosKPI
                .Where(r => r.CicloId == cicloId)
                .Select(r => r.UnidadeKPI)
                .Distinct()
                .Include(uk => uk.Unidade)
                .ToListAsync();
        }

    }
}



