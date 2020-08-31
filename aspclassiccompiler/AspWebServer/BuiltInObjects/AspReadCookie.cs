using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
// using ASPTypeLibrary;

namespace AspWebServer.BuiltInObjects
{
    class AspReadCookie // : IReadCookie
    {
        // private HttpCookie _cookie = null;
        private string _cookie;
            
        // public AspReadCookie(HttpCookie cookie)
        public AspReadCookie(string cookie)
        {
            _cookie = cookie;
        }

        #region IReadCookie Members

        public int Count
        {
            get { return _cookie == null ? 0 : 1; }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
            // return _cookie.Values.GetEnumerator();
        }

        public bool HasKeys
        {
            get { return _cookie == null ? false : true; }
        }

        public object get_Key(object VarKey)
        {
            throw new NotImplementedException();
        }

        public object this[object key]
        {
            get 
            {
                if (_cookie == null) return null;

                return _cookie; 
            }
        }
        #endregion

        public override string ToString()
        {
            return _cookie;
            // if (_cookie == null) return "";
            //
            // StringBuilder sb = new StringBuilder();
            // bool first = true;
            // foreach (string key in _cookie.Values.Keys)
            // {
            //     if (first)
            //     {
            //         first = false;
            //     }
            //     else
            //     {
            //         sb.Append('&');
            //     }
            //     sb.Append(key);
            //     sb.Append('=');
            //     sb.Append(_cookie[key]);
            // }
            // return sb.ToString();
        }
    }
}
