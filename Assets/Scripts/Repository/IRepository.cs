public interface IRepository<TKey, TValue>
{
    public void Add(TKey id, TValue data);
    TValue Get(TKey id);
    bool Exists(TKey id);
    void Clear();
}
