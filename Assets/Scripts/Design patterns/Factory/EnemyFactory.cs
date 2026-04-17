using UnityEngine;
using System.Collections.Generic;

public class EnemyFactory : SingletonMonoBehaviour<EnemyFactory>
{
    [SerializeField] [Required] private ObjectPool enemyPool;
    [SerializeField] private List<EnemySO> enemyConfig;
    private Dictionary<string, EnemySO> dataMapping = new Dictionary<string, EnemySO>();

    protected override void Awake()
    {
        base.Awake();
        
        foreach (var enemy in enemyConfig)
        {
            dataMapping[enemy.EnemyId] = enemy;
        }
    }

    public EnemyPresenter CreateEnemy(string enemyId, Transform parent)
    {
        if (dataMapping.TryGetValue(enemyId, out EnemySO enemyData))
        {
            EnemyPresenter enemyPresenter = enemyPool.GetObject().GetComponent<EnemyPresenter>();
            enemyPresenter.transform.SetParent(parent);

            enemyPresenter.SetUpEnemy(enemyData);

            return enemyPresenter;
        }

        Debug.LogError($"enemy ID {enemyId} not found in Factory!");
        return null;
    }
}