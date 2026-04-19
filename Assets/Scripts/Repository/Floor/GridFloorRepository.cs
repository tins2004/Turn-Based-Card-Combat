using System.Collections.Generic;
using UnityEngine;

public class GridFloorRepository : Singleton<GridFloorRepository>, IRepository<int, GameObject>
{
    private readonly Dictionary<int, GameObject> _data = new Dictionary<int, GameObject>();
    
    public void Add(int cell, GameObject obj)
    {
        _data[cell] = obj;
    }

    public GameObject Get(int cell)
    {
        return _data.TryGetValue(cell, out var obj) ? obj : null;
    }

    public List<GameObject> GetAllGameObjectCells()
    {
        List<GameObject> results = new List<GameObject>();
        
        for (int i = 0; i <  _data.Count; i++)
        {
            if (_data.TryGetValue(i, out var obj))
            {
                if (obj != null) results.Add(obj);
            }
        }
        
        return results;
    }

    public List<GameObject> GetGameObjectsByListIndex(List<int> cellsIndex)
    {
        List<GameObject> results = new List<GameObject>();
        
        foreach (var index in cellsIndex)
        {
            if (_data.TryGetValue(index, out var obj))
            {
                if (obj != null) results.Add(obj);
            }
        }
        
        return results;
    }

    public int GetTotalCells()
    {
        return _data.Count;
    }

    public bool Exists(int cell)
    {
       return _data.ContainsKey(cell);
    }

    public void Clear()
    {
        _data.Clear();
    }
}
