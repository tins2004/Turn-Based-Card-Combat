using UnityEngine;

public class EnemyModel : BaseActorModel
{
    public EnemySO enemyData
    {
        get { return actorData as EnemySO; }
    }

    public EnemyModel(EnemySO enemyData)
    {
        SetUpActor(100, enemyData);
    }
}
