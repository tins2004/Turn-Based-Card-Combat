using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ATTACK_BASE_ATTACK_ALGORITHM", menuName = "Asset/Skill Algorithm/Attack/Base Attack")]
public class BaseAttackStrategy : SkillStrategy
{
    private int currentCharacterCell;

    public override void Execute(BaseActorPresenter actor, int targetCell, SkillSO skillData)
    {
        actor.Attack(targetCell, skillData.ImpactValue);
    }

    public override List<int> GetRealCellsImpact()
    {
        List<int> results = new List<int>();

        currentCharacterCell = _model._actorOnFloorRepository.GetCellOfActorType(1)[0];
        int[] enemiesCell = _model._actorOnFloorRepository.GetCellOfActorType(2);
        int limitFloor = GridFloorRepository.Instance.GetTotalCells() - 1;

        foreach (int enemyCell in enemiesCell)
        {
            if (Math.Abs(enemyCell - currentCharacterCell) <= _model.skillData.RangeSkillImpact && enemyCell >= 0 && enemyCell <= limitFloor)
            {
                results.Add(enemyCell);
            }
        }

        return results;
    }

    public override List<int> GetCellsCanImpact(int realCellImpact) => new List<int> { realCellImpact };

    public override List<int> GetCellsOnLineImpact(int cellTarget) => new List<int> { currentCharacterCell };
}
