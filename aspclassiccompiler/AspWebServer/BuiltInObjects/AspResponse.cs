using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;

// using ASPTypeLibrary;

namespace AspWebServer.BuiltInObjects
{
	/// <summary>
	/// The response object that is accessible from ASP code
    /// Get not implement IResponse because the COM interface not supported by C#
	/// </summary>
	public class AspResponse //: IResponse
	{
        private readonly HttpContext _context;


        // public HttpContext _context;

		public AspResponse(HttpContext context)
        {
            _context = context;
            // _context=HttpContext.Current;
        }

		// public AspResponse(HttpContext context)
		// {_context=context;}

        #region IResponse Members

        public void Add(string bstrHeaderValue, string bstrHeaderName)
        {
            AddHeader(bstrHeaderName, bstrHeaderValue);
        }

        public void AddHeader(string name, string value)
            // { _context.Response.AddHeader(name, value); }
        {
            _context.Response.Headers.Add(name, value);
        }

        public void AppendToLog(string param)
            // { _context.Response.AppendToLog(param); }
        {
            // _context.
            Console.WriteLine(param);
        }

        public void BinaryWrite(object varInput)
        {
            _context.Response.Body.WriteAsync((byte[]) varInput);
            // _context.Response.BinaryWrite((byte[])varInput);
        }

        public void Clear()
        { _context.Response.Clear(); }

        public async void End()
        { await _context.Response.Body.DisposeAsync(); }

        public async void Flush()
        { await _context.Response.Body.FlushAsync(); }

        public bool IsClientConnected()
        {
            return _context.RequestAborted.IsCancellationRequested;
            // return _context.Response.IsClientConnected;
        }

        public void Pics(string value)
            // { _context.Response.Pics(value); }
        {
            // _context.Response.
            throw new NotImplementedException("Wut?");
        }

        public void Redirect(string url)
        {
            _context.Response.Redirect(url, true);
        }

        public void Write(object output)
        {
            if (output == null) return;

            string strOut = output as string;
            if (strOut == null)
            {
                Type t = output.GetType();
                while (t.IsCOMObject)
                {
                    output = t.InvokeMember(string.Empty,
                        BindingFlags.Default | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty,
                        null,
                        output,
                        null);
                    t = output.GetType();
                }

                strOut = output.ToString();
            }
            _context.Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes(strOut));
            // _context.Response.Body.WriteAsync(System.Text.Encoding.Unicode.GetBytes(output.ToString() ));
        }

        public void WriteBlock(short iBlockNumber)
        {
            throw new NotImplementedException();
        }

        private bool _buffering = true;
        public bool Buffer
        {
            get
            {
                throw new NotImplementedException();
            }
            // get { return _context.Response.Buffer; }
            // set { _context.Response.Buffer = value; }
            set
            {
                throw new NotImplementedException();

                // if (!value)
                // {
                //     var bufferingFeature = _context.Features.Get<IHttpResponseBodyFeature>();
                //     // bufferingFeature.DisableBuffering();
                //     // return bufferingFeature == null;
                //     bufferingFeature?.DisableBuffering();
                //     _buffering = false;
                // }
            }
        }

        public string CacheControl
        {
            // get { return _context.Response.CacheControl; }
            // set { _context.Response.CacheControl = value; }
            get
            {
                StringValues result;
                if (_context.Response.Headers.TryGetValue("Cache-Control", out result))
                {
                    return String.Join('\n', result.ToArray());
                };
                return result;
            }
        }

        public string CharSet
        {
            // What is the right spot to get this charset from?
            // get { return new System.Net.Mime.ContentType(_context.Response.ContentType).CharSet; }
            // get { return _context.Response.Headers["char-set"]; }
            // set { _context.Response.Headers["char-set"] = value;  }
            set
            {
                var next = new ContentType(_context.Response.ContentType);
                next.CharSet = value;
                _context.Response.ContentType = next.ToString();
            }
            // set { _context.Response.Charset = value; }
        }

        public int CodePage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ContentType
        {
            get { return _context.Response.ContentType; }
            set { _context.Response.ContentType = value; }
        }

        public AspCookieCollection Cookies
        {
            get
            {
                return new AspCookieCollection(null, _context.Response.Cookies);
            }
        }

        public int Expires
        {
            get
            {
                try
                {
                    return (int)this.ExpiresAbsolute.Subtract(DateTime.Now).TotalDays;
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
            // get { return _context.Response.Expires; }
            set { this.ExpiresAbsolute = DateTime.Now.AddDays(value); }
        }

        public DateTime ExpiresAbsolute
        {
            // get { return _context.Response.ExpiresAbsolute; }
            // set { _context.Response.ExpiresAbsolute = value; }
            get
            {
                var header = _context.Response.Headers["Expires"].First();
                DateTime date;
                if (header != null && DateTime.TryParse(header, out date))
                {
                    return date;
                }

                throw new InvalidDataException($"Expires header does not contain a valid date. Found: '${header}'");

            }
            set
            {
                _context.Response.Headers["Expires"] = value.ToString("R");
            }
        }

        public int LCID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Status
        {
            get { return _context.Response.StatusCode.ToString(); }
            set { throw new NotImplementedException(); }
            // set { _context.Response.Status = value; }
        }

        #endregion
    }
}
