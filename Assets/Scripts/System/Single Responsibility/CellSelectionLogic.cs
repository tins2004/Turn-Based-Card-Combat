using System.Collections.Generic;
using UnityEngine;

public class CellSelectionLogic
{
    public List<int> realImpactIndices { get; private set; } = new List<int>();
    public List<GameObject> realImpactObjects { get; private set; } = new List<GameObject>();
    public List<GameObject> currentActionAreaObjects { get; private set; } = new List<GameObject>();

    public List<int> canImpactIndices { get; private set; } = new List<int>();

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

    public void UpdateCanImpact(List<int> indices)
    {
        ClearCanImpact();
        canImpactIndices = indices;
    }

    public void ClearCanImpact()
    {
        canImpactIndices.Clear();
    }

    public int CellCanImpactNearTargetCell(int targetCell)
    {
        if (canImpactIndices == null || canImpactIndices.Count == 0) return -1;

        int nearestCell = canImpactIndices[0];
        int minDistance = int.MaxValue;

        foreach (int cell in canImpactIndices)
        {
            int distance = Mathf.Abs(cell - targetCell);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCell = cell;
            }
        }

        return nearestCell;
    }

    public void ClearCurrentActionArea()
    {
        currentActionAreaObjects.Clear();
    }
}