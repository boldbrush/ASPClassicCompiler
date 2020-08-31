using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
// using System.Web.SessionState;
using Microsoft.AspNetCore.Http;

namespace AspWebServer.BuiltInObjects
{
	/// <summary>
	/// The session object that is accessible from ASP code
	/// </summary>
	public class AspSession
	{
		// private HttpContext _context;
		private ISession _session;

		public AspSession(ISession session)
		{
			// _context=HttpContext.Current;
			_session = session;
		}

		// public AspSession(HttpContext context)
		// {
		// 	_context=context;
		// }

		public object this[string key]
		{
			// get {return _context.Session[key];}
			// set {_context.Session[key]=value;}
			get { return _session.Get(key); }
			set { _session.Set(key, (byte[]) value );}
		}

		public object StaticObjects
		{
			// get {return _context.Session["__Session.StaticObjects"];}
			get { return this["__Session.StaticObjects"]; }
		}

		// public HttpSessionState Contents
		public ISession Contents
		{
			get {return _session;}
		}

		public void Abandon()
		{
			// _context.Session.Abandon();
			// No more abandon. See https://github.com/aspnet/Session/issues/27
			_session.Clear();
		}

		public int CodePage
		{
			// get {return _context.Session.CodePage;}
			// set {_context.Session.CodePage=value;}
			get { throw new NotImplementedException("CodePage is an outdated property. Do we need it?"); }
			set { throw new NotImplementedException("CodePage is an outdated property. Do we need it?"); }
		}

		public int LCID
		{
			// get {return _context.Session.LCID;}
			// set {_context.Session.LCID=value;}
			get { return CultureInfo.CurrentCulture.LCID; }
			set { throw new NotImplementedException("Do we need LCID?"); }

		}

		public string SessionID
		{
			// get {return _context.Session.SessionID;}
			get { return _session.Id; }
		}

		public int TimeOut
		{
			// get {return _context.Session.Timeout;}
			// set {_context.Session.Timeout=value;}
			get { throw new NotImplementedException("Session timeout is not yet implemented."); }
			set { throw new NotImplementedException("Session timeout is not yet implemented."); }

		}
	}
}
