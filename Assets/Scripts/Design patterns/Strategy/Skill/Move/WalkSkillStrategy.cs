using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MOVE_WALK_ALGORITHM", menuName = "Asset/Skill Algorithm/Move/Walk")]
public class WalkSkillStrategy : SkillStrategy
{
    private int currentCharacterCell;

    public override void Execute(BaseActorPresenter actor, int targetCell, SkillSO skillData)
    {
        actor.MoveToCell(targetCell);
    }

    public override List<int> GetRealCellsImpact(int actorCell, int actorType)
    {
        List<int> results = new List<int>();

        currentCharacterCell = actorCell;
        int limitFloor = GridFloorRepository.Instance.GetTotalCells() - 1;

        for (int i = 0; i <= limitFloor; i++)
        {
            if (_model._actorOnFloorRepository.Exists(i)) continue;

            if (Math.Abs(i - currentCharacterCell) <= _model.skillData.RangeSkillImpact)
            {
                results.Add(i);
            }
        }

        return results;
    }

    public override List<int> GetCellsCanImpact(int realCellImpact) => new List<int> { realCellImpact };

    public override List<int> GetCellsOnLineImpact(int cellTarget) => new List<int> { currentCharacterCell };
}
