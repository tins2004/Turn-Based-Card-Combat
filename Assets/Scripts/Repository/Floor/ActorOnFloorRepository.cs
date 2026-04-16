using System.Collections.Generic;
using UnityEngine;

public class ActorOnFloorRepository : Singleton<ActorOnFloorRepository>, IRepository<int, int>
{
    private readonly Dictionary<int, int> _data = new Dictionary<int, int>();
    
    /// <summary>
    /// Actor Type: 0 is Space, 1 is Character, 2 is Enemy
    /// </summary>
    public void Add(int cell, int actorType)
    {
        if (actorType > 2) return;

        _data[cell] = actorType;
    }

    /// <summary>
    /// Actor Type: 0 is Space, 1 is Character, 2 is Enemy
    /// </summary>
    public int Get(int cell)
    {
        return _data.TryGetValue(cell, out var actorType) ? actorType : 0;
    }

    /// <summary>
    /// Actor Type: 0 is Space, 1 is Character, 2 is Enemy
    /// </summary>
    public int[] GetCellOfActorType(int actorType)
    {
        List<int> results = new List<int>();

        foreach (var pair in _data)
        {
            if (pair.Value == actorType)
            {
                results.Add((int)pair.Key);
            }
        }

        return results.ToArray();
    }
    
    /// <summary>
    /// This Cell is Space
    /// </summary>
    public bool Exists(int cell)
    {
        // foreach (var (i, val) in _data)
        // {
        //     Debug.Log($"Vị trí Cell: {i} có: {val}");
        // }

        if (_data.ContainsKey(cell))
        {   
            return _data[cell] != 0;
        }

        return false;
    }

    public void Clear()
    {
        _data.Clear();
    }
}
