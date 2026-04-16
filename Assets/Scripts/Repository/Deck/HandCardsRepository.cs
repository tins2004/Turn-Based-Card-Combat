using System.Collections.Generic;
using UnityEngine;

public class HandCardsRepository : Singleton<HandCardsRepository>, IRepository<string, CardPresenter>
{
    private readonly Dictionary<string, CardPresenter> _data = new Dictionary<string, CardPresenter>();
    
    public void Add(string name, CardPresenter obj)
    {
        _data[name] = obj;
    }

    public CardPresenter Get(string name)
    {
        return _data.TryGetValue(name, out var obj) ? obj : null;
    }

    public bool Exists(string id)
    {
       return _data.ContainsKey(id);
    }

    public void Clear()
    {
        _data.Clear();
    }
}
