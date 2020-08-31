using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AspWebServer.BuiltInObjects;
using Dlrsoft.VBScript;
using Dlrsoft.VBScript.Compiler;
using Dlrsoft.VBScript.Runtime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Scripting.Hosting;

namespace AspWebServer
{
    /**
     * This is the class that bridges ASP.NET CORE to the ClassicCompiler ASP
     */
    public class AspHandler
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _webRoot;
        private Dictionary<string, CompiledPage> _scriptCache = null;
        private AspHost _aspHost = null;
        
        public AspHandler(IWebHostEnvironment environment, string webRoot)
        {
            _environment = environment;
            _webRoot = webRoot;

            _scriptCache = new Dictionary<string, CompiledPage>();
            AspHostConfiguration config = new AspHostConfiguration();
            // config.Assemblies = AspHandlerConfiguration.Assemblies;
            // config.Trace = AspHandlerConfiguration.Trace;

            _aspHost = new AspHost(config);
        }

        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        private string PagePath(HttpContext context)
        {
            return Path.Join(_webRoot, context.Request.Path);
        }

        public async Task ProcessRequest(HttpContext context)
        {
            string pagePath = this.PagePath(context);
            await Console.Out.WriteLineAsync("servigin page " + pagePath);

            CompiledPage cpage = null;
            if (_scriptCache.ContainsKey(pagePath))
            {
                cpage = _scriptCache[pagePath];
                //don't use it if updated
                if (cpage.CompileTime < File.GetLastWriteTime(pagePath))
                    cpage = null;
            }
            
            if (cpage == null)
            {
                try
                {
                    cpage = _aspHost.ProcessPageFromFile(pagePath);
                    _scriptCache[pagePath] = cpage;
                }
                catch (VBScriptCompilerException ex)
                {
                    await AspHelper.RenderError(context.Response, ex);
                    return;
                }
            }

            ScriptScope pageScope = _aspHost.CreateScope();
            pageScope.SetVariable("response", new AspResponse(context));
            pageScope.SetVariable("request", new AspRequest(context));
            pageScope.SetVariable("session", new AspSession(context.Session));
            pageScope.SetVariable("server", new AspServer(_environment));
            pageScope.SetVariable("application", new AspApplication(context));
            AspObjectContext ctx = new AspObjectContext();
            pageScope.SetVariable("objectcontext", ctx);
            pageScope.SetVariable("getobjectcontext", ctx);
            //responseScope.SetVariable("err", new Microsoft.VisualBasic.ErrObject());
            //Used to get the literals
            pageScope.SetVariable("literals", cpage.Literals);

            try
            {
                object o = cpage.Code.Execute(pageScope);
            }
            catch (ThreadAbortException)
            {
                //Do nothing since we get this normally when calling response.redirect
            }
            catch (Exception ex)
            {
                if (_aspHost.Configuration.Trace)
                {
                    TraceHelper th = (TraceHelper)pageScope.GetVariable(VBScript.TRACE_PARAMETER);
                    string source = string.Format("{0} ({1},{2})-({3},{4})", th.Source, th.StartLine, th.StartColumn, th.EndLine, th.EndColumn);
                    throw new VBScriptRuntimeException(ex, source);
                }
                throw;
            }
        }

        #endregion
    }
}