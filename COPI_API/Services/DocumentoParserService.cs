using System.Text.RegularExpressions;

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
    }
}
