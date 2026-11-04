namespace lab_4_ShN.Data
{
    public interface ISaveList<T>
    {
        void Save(T data, string path);
        T Load(string path);
    }
}