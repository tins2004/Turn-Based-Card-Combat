using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ATTACK_BASE_ATTACK_ALGORITHM", menuName = "Asset/Skill Algorithm/Attack/Base Attack")]
public class BaseAttackStrategy : SkillStrategy
{
    private int currentCharacterCell;
    private int leftCellTarget;
    private int rightCellTarget;

    private int actorType;

    public override void Execute(BaseActorPresenter actor, int targetCell, SkillSO skillData)
    {
        actor.Attack(targetCell, skillData.ImpactValue);
    }

    public override List<int> GetRealCellsImpact(int actorCell, int actorType)
    {
        List<int> results = new List<int>();

        currentCharacterCell = actorCell;
        this.actorType = actorType;

        leftCellTarget = Math.Max(0, currentCharacterCell - _model.skillData.RangeSkillImpact);
        rightCellTarget = Math.Min(currentCharacterCell + _model.skillData.RangeSkillImpact, GridFloorRepository.Instance.GetTotalCells() - 1);

        if (leftCellTarget != currentCharacterCell) results.Add(leftCellTarget);
        if (rightCellTarget != currentCharacterCell) results.Add(rightCellTarget);

        return results;
    }

    public override List<int> GetCellsCanImpact(int realCellImpact)
    {
        List<int> results = new List<int>();
        
        int[] enemieCells = _model._actorOnFloorRepository.GetCellsByActorType(actorType == 1 ? 2 : 1);
        
        foreach (int enemyCell in enemieCells)
        {
            if ((enemyCell == leftCellTarget || enemyCell == rightCellTarget) && enemyCell != currentCharacterCell)
            {
                results.Add(enemyCell);
            }
        }
        
        return results;
    }

    public override List<int> GetCellsOnLineImpact(int cellTarget) => new List<int> { currentCharacterCell };
}
