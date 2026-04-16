using UnityEngine;

public struct CardActionORD
{
    public SkillStrategy skillStrategy;
    public int cellTarget;

    public CardActionORD(SkillStrategy skillStrategy, int cellTarget)
    {
        this.skillStrategy = skillStrategy;
        this.cellTarget = cellTarget;
    }
}