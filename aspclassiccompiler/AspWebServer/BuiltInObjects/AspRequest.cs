using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
// using ASPTypeLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace AspWebServer.BuiltInObjects
{
    public class AspRequest //: IRequest
    {
        private readonly HttpContext _context;
        // private HttpContext _context;

        #region constructor
		public AspRequest(HttpContext context)
        {
            _context = context;
            // _context= HttpContext.Current;
        }

		// public AspRequest(HttpContext context)
		// {
		// 	_context=context;
		// }
        #endregion

        #region IRequest Members

        public object BinaryRead(ref object pvarCountToRead)
        {
            byte[] buff = new byte[Convert.ToInt32(pvarCountToRead)];
            return _context.Request.Body.Read(buff, (int)_context.Request.Body.Position, Convert.ToInt32(pvarCountToRead));
            // return _context.Request.BinaryRead(Convert.ToInt32(pvarCountToRead));
        }

        public IRequestDictionary Body
        {
            get { throw new NotImplementedException(); }
        }

        public IRequestDictionary ClientCertificate
        {
            get { throw new NotImplementedException(); }
        }

        public IRequestDictionary Cookies
        {
            get { return new AspCookieCollection(_context.Request.Cookies, null); }
        }

        public IRequestDictionary Form
        {
            get
            {
                return new AspNameValueCollection(_context.Request.Form.ToNameValueCollection());
            }
        }

        public IRequestDictionary QueryString
        {
            get
            {
                var coll = _context.Request.QueryString.Value
                    .Split('&')
                    .Select(s =>
                        s.Split('=')
                            .Select(x =>
                                System.Net.WebUtility
                                    .UrlDecode(x)
                            )
                    ).ToDictionary(
                        el => el.First(),
                        el => new StringValues(el.Last())
                    ).ToNameValueCollection();
                return new AspNameValueCollection(coll);
            }
        }

        public IRequestDictionary ServerVariables
        {
            get
            {
                // this serves an outdated principle, see
                // https://stackoverflow.com/questions/38429604/how-to-access-servervariables-in-aspnetcore-1-0
                NameValueCollection vars = new NameValueCollection();
                Action<string, string> add = (string hdr, string key) =>
                {
                    if (_context.Request.Headers.Keys.Select(el => el.ToLower()).Contains(hdr.ToLower()))
                    {
                        vars.Add(key, _context.Request.Headers[hdr]);
                    }
                };
                add("Accept-Language", "HTTP_ACCEPT_LANGUAGE");
                add("X-Forwarded-For", "HTTP_X_FORWARDED_FOR");
                vars.Add("REMOTE_ADDR", _context.Connection.RemoteIpAddress.ToString());
                return new AspNameValueCollection(vars);
            }
        }

        public int TotalBytes
        {
            get { return Convert.ToInt32(_context.Request.ContentLength); }
        }

        public object this[string key]
        {
            get
            {
                if (_context.Request.Form.ContainsKey(key))
                    return _context.Request.Form[key];
                else if (this.QueryString[key] != null)
                    return this.QueryString[key];
                else
                    return "";
            }
        }

        #endregion
    }
}
