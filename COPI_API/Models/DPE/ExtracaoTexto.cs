namespace COPI_API.Models.DPE
{
    using System.Text.RegularExpressions;
    using UglyToad.PdfPig;

    public class PdfService
    {
        public string ExtrairTexto(string caminhoArquivo)
        {
            using var document = PdfDocument.Open(caminhoArquivo);

            var textoCompleto = "";

            foreach (var page in document.GetPages())
            {
                textoCompleto += page.Text + "\n";
            }

            return textoCompleto;
        }
        public string? ExtrairEmenta(string texto)
            {
                var match = Regex.Match(
                    texto,
                    @"Ementa:\s*(.+?)(\n|$)",
                    RegexOptions.IgnoreCase
                );

                return match.Success ? match.Groups[1].Value.Trim() : null;
            }
        };
        public class DocumentoParserService
        {
            public string? ExtrairEmenta(string texto)
            {
                var match = Regex.Match(texto, @"Ementa:\s*(.*)", RegexOptions.IgnoreCase);
                return match.Success ? match.Groups[1].Value.Trim() : null;
            }

            public string? ExtrairNumeroProcesso(string texto)
            {
                var match = Regex.Match(texto, @"Processo\s*:\s*(.*)", RegexOptions.IgnoreCase);
                return match.Success ? match.Groups[1].Value.Trim() : null;
            }
        }

}
