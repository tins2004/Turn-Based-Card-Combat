public class SKillModel
{
    public ActorOnFloorRepository _actorOnFloorRepository { get; private set; }
    public SkillSO skillData { get; private set; }


    public SKillModel(SkillSO skillData)
    {
        this.skillData = skillData;
        _actorOnFloorRepository = ActorOnFloorRepository.Instance;
    }
}
