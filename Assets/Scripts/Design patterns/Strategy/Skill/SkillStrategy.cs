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
