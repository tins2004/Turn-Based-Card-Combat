using System.Collections.Generic;
using UnityEngine;

public class CellSelectionLogic
{
    public List<int> realImpactIndices { get; private set; } = new List<int>();
    public List<GameObject> realImpactObjects { get; private set; } = new List<GameObject>();
    public List<GameObject> currentActionAreaObjects { get; private set; } = new List<GameObject>();

    private GridFloorRepository _repo => GridFloorRepository.Instance;

    public void UpdateRealImpact(List<int> indices)
    {
        ClearRealImpact();
        realImpactIndices = indices;
        foreach (int index in indices)
        {
            GameObject obj = _repo.Get(index);
            if (obj != null) realImpactObjects.Add(obj);
        }
    }

    public void ClearRealImpact()
    {
        realImpactIndices.Clear();
        realImpactObjects.Clear();
    }

    public void ClearCurrentActionArea()
    {
        currentActionAreaObjects.Clear();
    }
}