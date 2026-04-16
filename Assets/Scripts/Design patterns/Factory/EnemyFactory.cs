using UnityEngine;
using System.Collections.Generic;

public class EnemyFactory : SingletonMonoBehaviour<EnemyFactory>
{
    [System.Serializable]
    private struct EnemyConfig
    {
        [Required]
        public EnemySO enemyData;
        [Required]
        public GameObject enemyPrefab;
    }

    [SerializeField] private List<EnemyConfig> enemyConfig;
    private Dictionary<string, GameObject> prefabMapping = new Dictionary<string, GameObject>();
    private Dictionary<string, EnemySO> dataMapping = new Dictionary<string, EnemySO>();

    protected override void Awake()
    {
        base.Awake();
        
        foreach (var enemy in enemyConfig)
        {
            prefabMapping[enemy.enemyData.EnemyId] = enemy.enemyPrefab;
            dataMapping[enemy.enemyData.EnemyId] = enemy.enemyData;
        }
    }

    public EnemyPresenter CreateEnemy(string enemyId, Transform parent)
    {
        if (prefabMapping.TryGetValue(enemyId, out GameObject prefab))
        {
            EnemyPresenter enemyPresenter = Instantiate(prefab, parent).GetComponent<EnemyPresenter>();
            
            enemyPresenter.SetUpEnemy(dataMapping[enemyId]);

            return enemyPresenter;
        }

        Debug.LogError($"enemy ID {enemyId} not found in Factory!");
        return null;
    }
}