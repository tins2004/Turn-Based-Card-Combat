using UnityEngine;

public class EnemySystem : SingletonMonoBehaviour<EnemySystem>
{
    private string enemyObjectName = "Enemy";

    
    private void Start()
    {
        // SetupObserverListener();

        // List<string> startingEnemy = new List<string> { 
        //     "MOVE_DASH",
        //     "MOVE_DASH",
        //     "MOVE_TELEPORT",
        //     "MOVE_TELEPORT",
        //     "MOVE_TELEPORT",
        //     "MOVE_TELEPORT",
        //     "MOVE_DASH",
        //     "MOVE_DASH"
        // };

        // _model = new DeckModel(startingCards);

        SpawnEnemies("WARRIOR", 5);
        
    }

    private void SpawnEnemies(string enemyId, int cellTarget)
    {
        // for (int i = 0; i < _model.hand.Count; i++)
        // {
                CreateEnemy(enemyId, cellTarget, transform, $"{enemyObjectName} {0}");
            // EnemyPresenter card = CreateEnemy(enemyId, cellTarget, transform, $"{enemyObjectName} {0}");
        // }
    }

    public EnemyPresenter CreateEnemy(string enemyId, int cellTarget, Transform enemyParent, string name)
    {
        EnemyPresenter enemyPresenter = EnemyFactory.Instance.CreateEnemy(enemyId, enemyParent);

        if (enemyPresenter != null)
        {
            enemyPresenter.MoveToCell(cellTarget);
            enemyPresenter.name = name;

            return enemyPresenter;
        }

        return null;
    }
}
