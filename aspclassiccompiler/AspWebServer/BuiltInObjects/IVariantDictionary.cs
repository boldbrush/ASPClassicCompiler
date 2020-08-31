namespace AspWebServer.BuiltInObjects
{
    public interface IVariantDictionary
    {
        int Count { get; }
        System.Collections.IEnumerator GetEnumerator();
        void Remove(object VarKey);
        void RemoveAll();
        object get_Key(object VarKey);
        void let_Item(object VarKey, object pvar);
        object this[object VarKey] { get; set; }
    }
}