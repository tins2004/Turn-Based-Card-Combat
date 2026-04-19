using UnityEngine;

public class SkillCommand : ICommand
{
    private readonly SkillStrategy _skillStrategy;
    private readonly BaseActorPresenter actor;
    private readonly int targetCell;
    private readonly SkillSO skillData;

    public SkillCommand(SkillStrategy skillStrategy, BaseActorPresenter actor, int targetCell, SkillSO skillData)
    {
        _skillStrategy = skillStrategy;
        this.actor = actor;
        this.targetCell = targetCell;
        this.skillData = skillData;
    }

    public void Execute()
    {
        _skillStrategy.Execute(actor, targetCell, skillData);
    }
}