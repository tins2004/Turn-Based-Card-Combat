using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MOVE_DASH_ALGORITHM", menuName = "Asset/Skill Algorithm/Move/Dash")]
public class DashSkillStrategy : SkillStrategy
{
    private int currentCharacterCell;
    private int leftTargetToDash;
    private int rightTargetToDash;

    private int actorType;

    public override void Execute(BaseActorPresenter actor, int targetCell, SkillSO skillData)
    {
        actor.MoveToCell(targetCell);
    }

    public override List<int> GetRealCellsImpact(int actorCell, int actorType)
    {
        List<int> results = new List<int>();
        
        this.actorType = actorType;
        currentCharacterCell = actorCell;
        int limitFloor = GridFloorRepository.Instance.GetTotalCells() - 1;
        int range = _model.skillData.RangeSkillImpact;

        leftTargetToDash = Math.Max(0, currentCharacterCell - range);
        rightTargetToDash = Math.Min(limitFloor, currentCharacterCell + range);

        if (leftTargetToDash != currentCharacterCell) results.Add(leftTargetToDash);
        if (rightTargetToDash != currentCharacterCell) results.Add(rightTargetToDash);
        
        return results;
    }

    public override List<int> GetCellsCanImpact(int realCellImpact)
    {
        List<int> results = new List<int>();

        int[] enemieCells = _model._actorOnFloorRepository.GetCellsByActorType(actorType == 1 ? 2 : 1);
        int limitFloor = GridFloorRepository.Instance.GetTotalCells() - 1;

        int closestEnemyLeft = -1;
        int closestEnemyRight = limitFloor + 1;

        foreach (int enemyCell in enemieCells)
        {
            if (enemyCell >= leftTargetToDash && enemyCell < currentCharacterCell)
            {
                if (enemyCell > closestEnemyLeft)
                {
                    closestEnemyLeft = enemyCell;
                    leftTargetToDash = enemyCell + 1;
                }
            }
            else if (enemyCell > currentCharacterCell && enemyCell <= rightTargetToDash)
            {
                if (enemyCell < closestEnemyRight)
                {
                    closestEnemyRight = enemyCell;
                    rightTargetToDash = enemyCell - 1;
                }
            }
        }

        if (leftTargetToDash != currentCharacterCell && realCellImpact < currentCharacterCell) results.Add(leftTargetToDash);
        if (rightTargetToDash != currentCharacterCell && realCellImpact > currentCharacterCell) results.Add(rightTargetToDash);
        
        return results;
    }

    public override List<int> GetCellsOnLineImpact(int cellTarget)
    {
        List<int> results = new List<int>();

        int range = _model.skillData.RangeSkillImpact;
        int limitFloor = GridFloorRepository.Instance.GetTotalCells() - 1;

        int leftLimit = cellTarget < currentCharacterCell ? Math.Max(0, currentCharacterCell - range) : currentCharacterCell;
        int rightLimit = cellTarget < currentCharacterCell ? currentCharacterCell : Math.Min(limitFloor, currentCharacterCell + range);

        for (int i = leftLimit; i <= rightLimit; i++)
        {
            if (i != leftTargetToDash && i != rightTargetToDash)
            {
                results.Add(i);
            }
        }

        if (!results.Contains(currentCharacterCell))
        {
            results.Add(currentCharacterCell);
        }
        
        return results;
    }
}
