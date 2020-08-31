using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
// using ASPTypeLibrary;
using Microsoft.AspNetCore.Http;

namespace AspWebServer.BuiltInObjects
{
    public class AspCookieCollection : IRequestDictionary
    {
        private readonly IRequestCookieCollection _requestCookies;
        private readonly IResponseCookies _responseCookies;

        // private HttpCookieCollection _cookiecollection;
        private bool _request;

        #region constructor
        // public AspCookieCollection(HttpCookieCollection cookieCollection, bool request)
        public AspCookieCollection(IRequestCookieCollection requestCookies, IResponseCookies responseCookies)
        {
            _requestCookies = requestCookies;
            _responseCookies = responseCookies;
            // _cookiecollection = cookieCollection;
            // _request = request;
            _request = responseCookies == null;
        }
        #endregion

        #region IRequestDictionary Members

        public int Count
        {
            get { return _request ? _requestCookies.Count : 0; }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            // return _cookiecollection.GetEnumerator();
            return _request ? _requestCookies.GetEnumerator() : new ArrayList().GetEnumerator();
        }

        public object get_Key(object VarKey)
        {
            throw new NotImplementedException();
        }

        public object this[object key]
        {
            get
            {
                if (!_request)
                {
                    throw new NotImplementedException("Temporarily not implemented.");
                };
                // HttpCookie cookie = null;
                string cookie = null;
                if (key is int)
                    cookie = _requestCookies[((int) key).ToString()];
                else
                    cookie = _requestCookies[((string) key)];
            
                if (cookie == null)
                    return "";

                if (_request)
                    return new AspReadCookie(cookie);
                else
                    return null;
                // return new AspWriteCookie(cookie);
            }

            set
            {
                if (value == null || (value is string && string.IsNullOrEmpty((string)value)))
                {
                    if (!_request)
                    {
                        _responseCookies.Delete(key.ToString());
                    }
                    // _cookiecollection.Remove(key.ToString());
                }
            }
        }

        #endregion
    }
}
