using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MOVE_DASH_ALGORITHM", menuName = "Asset/Skill Algorithm/Move/Dash")]
public class DashSkillStrategy : SkillStrategy
{
    private int leftTargetToDash;
    private int rightTargetToDash;

    public override List<int> GetRealCellsImpact()
    {
        List<int> results = new List<int>();

        int currentCell = _model._actorOnFloorRepository.GetCellOfActorType(1)[0];
        int limitFloor = GridFloorRepository.Instance.GetTotalCells() - 1;
        int range = _model.skillData.RangeSkillImpact;

        leftTargetToDash = Math.Max(0, currentCell - range);
        rightTargetToDash = Math.Min(limitFloor, currentCell + range);


        if (leftTargetToDash != currentCell) results.Add(leftTargetToDash);
        if (rightTargetToDash != currentCell) results.Add(rightTargetToDash);
        
        return results;
    }

    public override List<int> GetCellsCanImpact(int realCellImpact)
    {
        List<int> results = new List<int>();

        int currentCell = _model._actorOnFloorRepository.GetCellOfActorType(1)[0];
        int[] enemiesCell = _model._actorOnFloorRepository.GetCellOfActorType(2);
        int limitFloor = GridFloorRepository.Instance.GetTotalCells() - 1;

        int closestEnemyLeft = -1;
        int closestEnemyRight = limitFloor + 1;

        foreach (int enemyCell in enemiesCell)
        {
            if (enemyCell >= leftTargetToDash && enemyCell < currentCell)
            {
                if (enemyCell > closestEnemyLeft)
                {
                    closestEnemyLeft = enemyCell;
                    leftTargetToDash = enemyCell + 1;
                }
            }
            else if (enemyCell > currentCell && enemyCell <= rightTargetToDash)
            {
                if (enemyCell < closestEnemyRight)
                {
                    closestEnemyRight = enemyCell;
                    rightTargetToDash = enemyCell - 1;
                }
            }
        }

        if (leftTargetToDash != currentCell && realCellImpact < currentCell) results.Add(leftTargetToDash);
        if (rightTargetToDash != currentCell && realCellImpact > currentCell) results.Add(rightTargetToDash);
        
        return results;
    }

    public override List<int> GetCellsOnLineImpact(int cellTarget)
    {
        List<int> results = new List<int>();

        int currentCell = _model._actorOnFloorRepository.GetCellOfActorType(1)[0];
        int range = _model.skillData.RangeSkillImpact;
        int limitFloor = GridFloorRepository.Instance.GetTotalCells() - 1;

        int leftLimit = cellTarget < currentCell ? Math.Max(0, currentCell - range) : currentCell;
        int rightLimit = cellTarget < currentCell ? currentCell : Math.Min(limitFloor, currentCell + range);

        for (int i = leftLimit; i <= rightLimit; i++)
        {
            if (i != leftTargetToDash && i != rightTargetToDash)
            {
                results.Add(i);
            }
        }

        if (!results.Contains(currentCell))
        {
            results.Add(currentCell);
        }
        
        return results;
    }
}
