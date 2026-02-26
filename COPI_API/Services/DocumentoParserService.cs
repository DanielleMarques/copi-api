using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace COPI_API.Services
{
    public class DocumentoParserService
    {
        public string? ExtrairEmenta(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return null;

            // EXPLICAÇÃO DO NOVO PADRÃO:
            // (?i) -> Case Insensitive
            // Ementa[:\s]+ -> Começa após "Ementa:"
            // ([\s\S]+?) -> Captura tudo de forma "preguiçosa"
            // (?= ... ) -> ATÉ encontrar um dos seguintes (sem incluir na captura):

            // 1. Interessad[oa] seguido de dois pontos (ex: Interessada:)
            // 2. Assunto seguido de dois pontos
            // 3. 1. RELATÓRIO (início de tópico)

            var padrao = @"(?i)Ementa[:\s]+([\s\S]+?)(?=\s*Interessad[ao]\s*:|\s*Assunto\s*:|1\.\s?RELATÓRIO|$)";

            var match = Regex.Match(texto, padrao);

            if (match.Success)
            {
                var extraido = match.Groups[1].Value.Trim();

                // Limpeza: remove quebras de linha que o PDF insere no meio das frases
                extraido = Regex.Replace(extraido, @"\s+", " ");

                // Limpeza: remove pontos ou traços que ficam sobrando no final
                extraido = Regex.Replace(extraido, @"[-\s\.]+$", "");

                return extraido.Trim();
            }

            return null;
        }

        public string? ExtrairNumeroSei(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return null;

            // Busca o padrão: 4 dígitos . 4 dígitos / 7 dígitos - 1 dígito
            // Ex: 6019.2025/0000922-7
            var padrao = @"\d{4}\.\d{4}/\d{7}-\d{1}";
            var match = Regex.Match(texto, padrao);

            return match.Success ? match.Value : null;
        }

        public string? ExtrairNumeroProcesso(string texto)
        {
            var match = Regex.Match(
                texto,
                @"Processo\s*:\s*(.+?)(\n|$)",
                RegexOptions.IgnoreCase
            );

            return match.Success
                ? match.Groups[1].Value.Trim()
                : null;
        }

        public string? ExtrairDataAssinaturaFinal(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return null;

            // Normaliza espaços para evitar quebras de linha entre "Em" e a data
            string textoLimpo = Regex.Replace(texto, @"\s+", " ");

            // Busca a data que acompanha o padrão de assinatura eletrônica do SEI
            // Ex: "Em 12/02/2025, às 19:06."
            var match = Regex.Match(textoLimpo, @"Em\s+(\d{2}/\d{2}/\d{4}),\s+às", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            // Fallback: Se não houver o selo padrão, pega a última data encontrada no documento
            var lastDate = Regex.Match(textoLimpo, @"(\d{2}/\d{2}/\d{4})", RegexOptions.RightToLeft);

            return lastDate.Success ? lastDate.Groups[1].Value : null;
        }

        private (string nome, string situacao) ExtrairNomeESituacao(string texto)
        {
            var regex = new Regex(
                @"1\.?\s*Nome\s+completo\s*2\.?\s*Situaç[ãa]o\s+funcional\s*(.*?)\s+([A-Za-zÀ-ÿ]+)\s*(?=3\.)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );

            var match = regex.Match(texto);

            if (!match.Success)
                return (null, null);

            string nome = match.Groups[1].Value.Trim();
            string situacao = match.Groups[2].Value.Trim();

            return (nome, situacao);
        }

        public object ExtrairDadosCompletosDeclaracao(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return null;

            // Normalizamos o texto removendo quebras de linha e espaços duplos
            string textoNormalizado = Regex.Replace(texto, @"\s+", " ");

            textoNormalizado = Regex.Replace(textoNormalizado, @"([a-zA-ZÀ-ÿ])(\d+\.)", "$1 $2");

            textoNormalizado = Regex.Replace(textoNormalizado, @"([a-zA-ZÀ-ÿ])([IVXLCDM]+\.)", "$1 $2");

            string textoLimpo = Regex.Replace(texto, @"DECLARAÇÃO DE MOTIVAÇÃO.*?pg\.\s*\d+", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var (nome, situacao) = ExtrairNomeESituacao(textoNormalizado);

            return new
            {
                // --- SEÇÃO 1: DADOS DO SERVIDOR ---
                nomeCompleto = nome,
                situacaoFuncional = situacao,
                rf = ExtrairCampoLapidado(textoNormalizado, @"(?:3\.\s*)?RF"),
                cargo = ExtrairCargoEspecifico(textoNormalizado),
                orgao = ExtrairCampoLapidado(textoNormalizado, @"(?:5\.\s*)?Órgão\s+ou\s+Entidade"),

                // --- SEÇÃO 2: INFORMAÇÕES DO EVENTO ---             
                tipoEvento = ExtrairCampoLapidado(textoNormalizado, @"(?:I\.\s*)?Tipo\s+de\s+evento(?:\s*\(.*?\))?"),
                nomeEvento = ExtrairCampoLapidado(textoNormalizado, @"(?:II\.\s*)?Nome\s+do\s+evento"),
                dataInicioFim = ExtrairCampoLapidado(textoNormalizado, @"(?:III\.\s*)?Data\s+de\s+início(?:\s*e\s*encerramento)?(?:\s*/\s*data\s+de\s+encerramento)?"),
                dataIdaVolta = ExtrairCampoLapidado(textoNormalizado, @"(?:IV\.\s*)?Data\s+de\s+ida(?:\s*e\s*volta)?(?:\s*/\s*data\s+de\s+volta)?"),
                cidade = ExtrairCampoLapidado(textoNormalizado, @"(?:V\.\s*)?Cidade"),
                pais = ExtrairCampoLapidado(textoNormalizado, @"(?:VI\.\s*)?País"),

                // --- SEÇÃO 3: ORGANIZAÇÃO E DETALHES ---
                tipoParticipacao = ExtrairCampoLapidado(textoNormalizado, @"(?:XII\.\s*)?Tipo\s+de\s+participaç[ãa]o(?:\s*\(.*?\))?"),
                patrocinadores = ExtrairCampoLapidado(textoNormalizado, @"(?:VIII\.\s*)?Patrocinador(?:\s*\(es\))?\s*/\s*Apoiadores"),
                organizador = ExtrairCampoLapidado(textoNormalizado,@"(?:VII\.\s*)?Organizador\s*(?:\(\s*es\s*\))?\s*:?"),

                descricaoEvento = ExtrairCampoLapidado(textoNormalizado, @"(?:IX\.\s*Breve\s+descriç[ãa]o\s+do\s+evento|Descriç[ãa]o\s+do\s+[Ee]vento|Breve\s+descriç[ãa]o)"),
                motivacaoEvento = ExtrairCampoLapidado(
                    textoNormalizado,
                    @"(?:XI\.?\s*)?Motiva[cç][aã]o\s*/\s*relev[âa]ncia.*?participa[cç][aã]o"
                )
        };
        }

        private string ExtrairCampoLapidado(string textoAlvo, string patternInicio, string patternLimiteCustom = null)
        {
            string patternLimite = @"(?=\s\d+\.\s|\s[IVXLCDM]+\.\s|Documento|SEI|$)";

            var regex = new Regex(
                patternInicio + @"\s*[:\-]?\s*(.*?)" + patternLimite,
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );

            var match = regex.Match(textoAlvo);
            if (!match.Success) return null;

            return match.Groups[1].Value.Trim();
        }

        private string ExtrairCargoEspecifico(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return null;

            // 1. Localiza o início após "Cargo" (item 3.1)
            // 2. O conteúdo vai até encontrar o marcador "5." (Órgão ou Entidade) ou o final do texto
            // Usamos um Lookahead positivo para o "5." ou "Órgão"
            var regexCargo = new Regex(
                @"(?i)3\.1\s*Cargo\s*[:\-]?\s*(.*?)(?=\s+5\.\s+|Órgão\s+ou\s+Entidade|$)",
                RegexOptions.Singleline
            );

            var match = regexCargo.Match(texto);

            if (match.Success)
            {
                string cargo = match.Groups[1].Value.Trim();

                // Limpeza final: Se sobrar algum resíduo do número 5 no final, removemos
                cargo = Regex.Replace(cargo, @"\s+5\.?$", "");

                return cargo;
            }

            return null;
        }
    }
}
