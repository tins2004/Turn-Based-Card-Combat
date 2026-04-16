using System.Collections.Generic;
using UnityEngine;

public abstract class SkillStrategy : ScriptableObject
{
    protected SKillModel _model { get; private set; }

    public void ConfigDataSKill(SkillSO skillData)
    {
        _model = new SKillModel(skillData);
    }

    public SkillSO GetSkillData()
    {
        return _model.skillData;
    }
    
    /// <summary>
    /// Execute the skill's effect on the target cell, applying damage, healing, or other effects as defined by the skill's logic.
    /// </summary>
    public abstract void Execute(BaseActorPresenter actor, int targetCell, SkillSO skillData);

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Array Cells follow range of card</returns>
    public abstract List<int> GetRealCellsImpact();

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Array Cells allow the Card may have an impact</returns>
    public abstract List<int> GetCellsCanImpact(int realCellImpact);

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Array Cells actor or subject need to pass through</returns>
    public abstract List<int> GetCellsOnLineImpact(int cellTarget);
}
