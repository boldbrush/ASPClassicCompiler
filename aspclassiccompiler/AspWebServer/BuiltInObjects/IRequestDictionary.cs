using System.Collections;

namespace AspWebServer.BuiltInObjects
{
    public interface IRequestDictionary
    {
        int Count { get; }
        IEnumerator GetEnumerator();
        object get_Key(object VarKey);
        object this[object key] { get; }
    }
}