using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Extensions.Primitives;

// using ASPTypeLibrary;

namespace AspWebServer.BuiltInObjects
{
    public static class AspCollectionExtensions
    {
        public static NameValueCollection ToNameValueCollection(this IEnumerable<KeyValuePair<string, StringValues>> coll)
        {
            NameValueCollection c = new NameValueCollection();
            foreach (var item in coll)
            {
                foreach (var value in item.Value)
                {
                    c.Add(item.Key, value);
                }
            }
            return c;
        }
    }
    class AspNameValueCollection : IRequestDictionary
    {
        private NameValueCollection _collection;

        public AspNameValueCollection(NameValueCollection collection)
        {
            _collection = collection;
        }

        public override string ToString()
        {
            return _collection.ToString();
        }

        #region IRequestDictionary Members

        public int Count
        {
            get { return _collection.Count; }
        }

        public IEnumerator GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public object get_Key(object VarKey)
        {
            throw new NotImplementedException();
        }

        public object this[object key]
        {
            get 
            {
                string[] values = null;
                if (key is int)
                {
                    values = _collection.GetValues((int)key - 1);
                }
                else if (key is string)
                {
                    //Serveral keys are supported by ASP but not ASP.NET so we have to map it
                    string ucKey = ((string)key).ToUpper();
                    // this is really weird. why are we mapping specialized keys inside a generalized collection class?
                    switch ((string)ucKey)
                    {
                        case "HTTP_METHOD":
                            ucKey = "REQUEST_METHOD";
                            break;

                        case "HTTP_URL":
                            ucKey = "URL";
                            break;

                        case "HTTP_VERSION":
                            ucKey = "SERVER_PROTOCOL";
                            break;
                    }
                    values = _collection.GetValues(ucKey);
                }
                else
                {
                    throw new ArgumentException("Key has to be integer or string");
                }

                if (values == null)
                {
                    values = new string[] {};
                }
                return new AspStringList(values);
            }
        }

        #endregion
    }
}
