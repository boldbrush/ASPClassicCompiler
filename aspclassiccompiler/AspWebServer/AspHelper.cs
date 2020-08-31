using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Dlrsoft.VBScript.Compiler;
using Microsoft.AspNetCore.Http;

namespace AspWebServer
{
    public class AspHelper
    {
        public static async Task RenderError(HttpResponse response, VBScriptCompilerException exception)
        {
            response.Clear();
            response.StatusCode = 500;
            // response.
            // response.StatusDescription = "Internal Server Error";

            await writeErrorOutput(response, exception);
            // response.Body.EndWrite();
        }

        public static async Task writeErrorOutput(HttpResponse output, VBScriptCompilerException exception)
        {
            await output.WriteAsync("<h1>VBScript Compiler Error</h1>");
            await output.WriteAsync("<table>");
            await output.WriteAsync("<tr>");
            await output.WriteAsync(string.Format("<td>{0}</td>", "FileName"));
            await output.WriteAsync(string.Format("<td>{0}</td>", "Line"));
            await output.WriteAsync(string.Format("<td>{0}</td>", "Column"));
            await output.WriteAsync(string.Format("<td>{0}</td>", "Error Code"));
            await output.WriteAsync(string.Format("<td>{0}</td>", "Error Description"));
            await output.WriteAsync("</tr>");
            foreach (VBScriptSyntaxError error in exception.SyntaxErrors)
            {
                await output.WriteAsync("<tr>");
                await output.WriteAsync(string.Format("<td>{0}</td>", error.FileName));
                await output.WriteAsync(string.Format("<td>{0}</td>", error.Span.Start.Line));
                await output.WriteAsync(string.Format("<td>{0}</td>", error.Span.Start.Column));
                await output.WriteAsync(string.Format("<td>{0}</td>", error.ErrorCode));
                await output.WriteAsync(string.Format("<td>{0}</td>", error.ErrorDescription));
                await output.WriteAsync("</tr>");
            }
            await output.WriteAsync("</table>");
        }
    }
}
