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
