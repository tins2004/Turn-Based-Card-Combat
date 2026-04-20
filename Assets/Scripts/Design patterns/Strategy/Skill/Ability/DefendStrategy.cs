using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ABILITY_DEFEND_ALGORITHM", menuName = "Asset/Skill Algorithm/Ability/Defend")]
public class DefendStrategy : SkillStrategy
{
    public override void Execute(BaseActorPresenter actor, int targetCell, SkillSO skillData)
    {
        actor.AddShield(skillData.ImpactValue);
    }

    public override List<int> GetRealCellsImpact(int actorCell, int actorType) => new List<int> {actorCell};

    public override List<int> GetCellsCanImpact(int realCellImpact) => new List<int> {realCellImpact};

    public override List<int> GetCellsOnLineImpact(int cellTarget) => null;
}
