using System.Collections.Generic;

public class DeckRepository : Singleton<DeckRepository>, IRepository<int, float>
{
    private readonly Dictionary<int, float> _data = new Dictionary<int, float>();
    
    public void Add(int id, float xPos)
    {
        _data[id] = xPos;
    }

    public float Get(int id)
    {
        return _data.TryGetValue(id, out var xPos) ? xPos : 0f;
    }

    public bool Exists(int id)
    {
       return _data.ContainsKey(id);
    }

    public void Clear()
    {
        _data.Clear();
    }
}
