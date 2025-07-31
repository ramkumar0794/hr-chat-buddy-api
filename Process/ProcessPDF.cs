using ClientManager.OpenAI;
using Microsoft.Extensions.Configuration;
using Process.Interface;
using System.Text;
using UglyToad.PdfPig;

namespace Process
{
    public class ProcessPDF : IProcessPDF
    {
        public string ExtractText(string path)
        {
            var text = new StringBuilder();
            using(var document = PdfDocument.Open(path))
            {
                foreach(var page in document.GetPages())
                {
                    text.AppendLine(page.Text);
                }
            }
            return text.ToString();
        }

        
    }
}
