using System.Text;
using UglyToad.PdfPig;

namespace COPI_API.Services
{
    public class PdfService
    {
        public string ExtrairTexto(string caminhoArquivo)
        {
            using var document = PdfDocument.Open(caminhoArquivo);
            var textoCompleto = new StringBuilder();

            foreach (var page in document.GetPages())
            {
                // GetWords tenta agrupar o texto de forma mais natural para leitura humana
                var letters = page.Letters;
                textoCompleto.AppendLine(page.Text);
            }

            return textoCompleto.ToString();
        }
    }
}
