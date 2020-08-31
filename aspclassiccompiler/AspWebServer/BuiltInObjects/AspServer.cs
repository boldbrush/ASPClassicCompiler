using System;
using System.IO;
using System.Web;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;

// using AspWebServer.BuiltInObjects;

// using ASPTypeLibrary;

namespace AspWebServer.BuiltInObjects
{
	/// <summary>
	/// The server object that is accessible from ASP code
	/// </summary>
	public class AspServer // : IServer
	{
		private readonly IWebHostEnvironment _environment;

		// private HttpContext _context;
        private int _scriptTimeout;

		public AspServer(IWebHostEnvironment environment)
		{
			_environment = environment;
			// _context=HttpContext.Current;
		}

		// public AspServer(HttpContext context)
		// {
		// 	_context=context;
		// }

		public object CreateObject(string literal)
		{
			//return _context.Server.CreateObject(literal);
            Type t = Type.GetTypeFromProgID(literal);
            if (t == null)
            {
                throw new Exception(string.Format("Cannot create server object ''{0}", literal));
            }

            return Activator.CreateInstance(t);
		}

		public void Execute(string path)
		{
			throw new NotImplementedException("ASPServer.Execute is not implemented");
		}

		private AspError _error=new AspError();
        public AspError LastError
		{
			get {return _error;}
			set {_error=value;}
		}

        public AspError GetLastError()
		{
			return _error;
		}

		public string HTMLEncode(string s)
		{
			// return _context.Server.HtmlEncode(s);
			return System.Net.WebUtility.HtmlEncode(s);
		}

		public string MapPath(string path)
		{
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("Path is required for the Server.MapPath function.");

            return Path.Join(_environment.WebRootPath, path);
            // return _context.Server.MapPath(path);
		}

		public void Transfer(string path)
		{
			// _context.Server.Transfer(path, true);
			throw new NotImplementedException();
		}

		public string URLEncode(string s)
		{
			// return _context.Server.UrlEncode(s);
			return System.Net.WebUtility.UrlEncode(s);
		}

        public string URLPathEncode(string s)
        {
	        return System.Net.WebUtility.UrlEncode(s);

            // return _context.Server.UrlPathEncode(s);
        }

		public int ScriptTimeout
		{
            get { return _scriptTimeout; }
            set { _scriptTimeout = value; }
		}
	}
}
