using System;
using System.Collections;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace AspWebServer.BuiltInObjects
{
	/// <summary>
	/// A cookie for use with asp scripts
	/// </summary>
	public class AspCookie : ICollection
	{
		private CookieBuilder _cookie = null;
		private string _value = null;
		public CookieBuilder HttpCookie
		{
			get { return _cookie; }
			set { _cookie=value; }
		}

		public AspCookie(CookieBuilder cookie, string value = null)
		{
			_cookie = cookie;
			_value = value;
		}

		public string this[string subkey]
		{
			get { return _cookie==null ? "" : _value; }
			set { _value = value;}
		}

		public bool HasKeys
		{
			get { return _cookie==null; }
		}

		public string Value
		{
			get {return _value==null ? "" : _value;}
			set {_value = value;}
		}

		public DateTime Expires
		{
			// get {return _cookie.Expires;}
			// set {_cookie.Expires=value;}
			get { return DateTime.Now.Add(_cookie.Expiration.Value); }
			set { _cookie.Expiration = value.Subtract(DateTime.Now); }
		}

		public string Domain
		{
			get {return _cookie.Domain;}
			set {_cookie.Domain=value;}
		}

		public string Name
		{
			get {return _cookie==null?"":_cookie.Name;}
			set {_cookie.Name=value;}
		}

		public string Path
		{
			get {return _cookie==null?"":_cookie.Path;}
			set {_cookie.Path=value;}
		}

		public bool Secure
		{
			get {return _cookie != null && (_cookie.SecurePolicy != CookieSecurePolicy.None) ;}
			set {_cookie.SecurePolicy = value ? CookieSecurePolicy.Always : CookieSecurePolicy.None; }
		}

		// ICollection implementation
		public int Count
		{
			get { return _cookie==null ? 0 : 1 ;}
		}

		public bool IsSynchronized
		{
			get {return false;}
		}

		public object SyncRoot
		{
			get { return _cookie; }
		}

		public void CopyTo(Array array, int index)
		{
			new ArrayList() {_value}.CopyTo(array);
			
			// if (_cookie.HasKeys)
			// 	_cookie.Values.CopyTo(array, index);
			// else
			// 	array.SetValue(_cookie.Value, index);
		}

		// IEnumerable implementation
		public IEnumerator GetEnumerator()
		{
			return new ArrayList(){_value}.GetEnumerator();
			// return new CookieEnumerator(_cookie);
		}

		// public class CookieEnumerator : IEnumerator
		// {
		// 	private IEnumerator _cookieenum=null;
		// 	public CookieEnumerator(HttpCookie cookie)
		// 	{
		// 		_cookieenum=cookie.Values.Keys.GetEnumerator();
		// 	}
		//
		// 	public object Current
		// 	{
		// 		get {return _cookieenum.Current;}
		// 	}
		//
		// 	public bool MoveNext()
		// 	{
		// 		return _cookieenum.MoveNext();
		// 	}
		//
		// 	public void Reset()
		// 	{
		// 		_cookieenum.Reset();
		// 	}
		// }
	}
}
