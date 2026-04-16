using UnityEngine;

public class EnemyModel : BaseActorModel
{
    public EnemySO enemyData { get; private set; }

    public EnemyModel(EnemySO enemyData)
    {
        this.enemyData = enemyData;

        SetUpActor(100);
    }
}
