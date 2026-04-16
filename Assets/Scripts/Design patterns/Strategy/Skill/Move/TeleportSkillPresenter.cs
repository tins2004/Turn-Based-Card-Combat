using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MOVE_TELEPORT_ALGORITHM", menuName = "Asset/Skill Algorithm/Move/Teleport")]
public class TeleportSkillStrategy : SkillStrategy
{
    public override void Execute(BaseActorPresenter actor, int targetCell, SkillSO skillData)
    {
        actor.MoveToCell(targetCell);
    }
    
    public override List<int> GetRealCellsImpact()
    {
        List<int> results = new List<int>();

        int limitFloor = GridFloorRepository.Instance.GetTotalCells() - 1;

        for (int i = 0; i <= limitFloor; i++)
        {
            if (_model._actorOnFloorRepository.Exists(i)) continue;

            results.Add(i);
        }

        return results;
    }

    public override List<int> GetCellsCanImpact(int realCellImpact) => new List<int> { realCellImpact };

    public override List<int> GetCellsOnLineImpact(int cellTarget) => new List<int> { _model._actorOnFloorRepository.GetCellOfActorType(1)[0] };
}
