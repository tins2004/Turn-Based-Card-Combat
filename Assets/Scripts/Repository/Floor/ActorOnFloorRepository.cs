using System.Collections.Generic;
using UnityEngine;

public struct ActorOnFloorData
{
    /// <summary>
    /// Actor Type: 0 is Space, 1 is Character, 2 is Enemy
    /// </summary>
    public int actorType;
    public BaseActorPresenter actorObject;
}

public class ActorOnFloorRepository : Singleton<ActorOnFloorRepository>, IRepository<int, ActorOnFloorData>
{

    private readonly Dictionary<int, int> _data = new Dictionary<int, int>();
    private readonly Dictionary<int, BaseActorPresenter> _actorObjects = new Dictionary<int, BaseActorPresenter>();
    
    /// <summary>
    /// Actor Type: 0 is Space, 1 is Character, 2 is Enemy
    /// </summary>
    public void Add(int cell, ActorOnFloorData actorOnFloorData)
    {
        if (actorOnFloorData.actorType > 2) return;

        _data[cell] = actorOnFloorData.actorType;

        if (actorOnFloorData.actorType != 0 && actorOnFloorData.actorObject != null)
        {
            _actorObjects[cell] = actorOnFloorData.actorObject;
        }
    }

    /// <summary>
    /// Actor Type: 0 is Space, 1 is Character, 2 is Enemy
    /// </summary>
    public ActorOnFloorData Get(int cell)
    {
        return _data.TryGetValue(cell, out var actorType) ? new ActorOnFloorData { actorType = actorType, actorObject = GetActorObject(cell) } : new ActorOnFloorData { actorType = 0, actorObject = null };
    }

    public int GetActorType(int cell)
    {
        return _data.TryGetValue(cell, out var actorType) ? actorType : 0;
    }

    public BaseActorPresenter GetActorObject(int cell)
    {
        return _actorObjects.TryGetValue(cell, out var actorObject) ? actorObject : null;
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
