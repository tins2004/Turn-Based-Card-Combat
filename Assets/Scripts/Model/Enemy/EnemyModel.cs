using UnityEngine;

public class EnemyModel : BaseEntity
{
    public EnemySO enemyData { get; private set; }

    private const float YWORDPOSITION = 1.15f;
    public int currentEnemyCell { get; set; }
    public ActorOnFloorRepository _actorOnFloorRepository { get; private set; }
    public GridFloorRepository _gridFloorRepository { get; private set; }

    public EnemyModel(EnemySO enemyData)
    {
        this.enemyData = enemyData;

        _actorOnFloorRepository = ActorOnFloorRepository.Instance;
        _gridFloorRepository = GridFloorRepository.Instance;

        SetUpEntity(100);
    }

    public Vector2? GetTargetPosition(int targetCellIndex)
    {
        if (!_gridFloorRepository.Exists(targetCellIndex))
        {
            Debug.LogWarning($"Cell index {targetCellIndex} does not exist in Repository.");
            return null;
        }
        
        return new Vector2(_gridFloorRepository.Get(targetCellIndex).transform.localPosition.x, YWORDPOSITION);
    }
}
