using System;
using System.Collections;
using System.Web;
// using ASPTypeLibrary;

namespace AspWebServer.BuiltInObjects
{
	/// <summary>
	/// A cookie for use with asp scripts
	/// </summary>
	public class AspWriteCookie : IWriteCookie
	{
		// private HttpCookie _cookie=null;
        private Microsoft.AspNetCore.Http.CookieOptions _cookie = null;
        private string _value = null;
        
        // private System.Web.HttpUtility
		public AspWriteCookie(Microsoft.AspNetCore.Http.CookieOptions cookie, string value = null)
		{
			_cookie = cookie;
            _value = value;
        }

        #region IWriteCookie Members

        public IEnumerator GetEnumerator()
        {
            //return new CookieEnumerator(_cookie);
            // _cookie.
            // return _cookie.Values.GetEnumerator();
            
            return null;
        }

        public string Domain
        {
            set { _cookie.Domain = value; }
        }

        public DateTime Expires
        {
            set { _cookie.Expires = value; }
        }

        /**
         * This is part of a legacy, non-standard API that is deprecated in Core
         */
        public bool HasKeys
        {
            get { return _cookie != null && _value != null; }
        }

        public string Path
        {
            set { _cookie.Path = value; }
        }

        public bool Secure
        {
            set { _cookie.Secure = value; }
        }

        public string this[object Key]
        {
            set { _value = value; }
        }

        #endregion
    }
}
