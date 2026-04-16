using UnityEngine;

public class CharacterModel : BaseEntity
{
    private const float Y_WORD_POSITION = 1.15f;
    public int currentCharacterCell { get; set; }

    public ActorOnFloorRepository _actorOnFloorRepository { get; private set; }
    public GridFloorRepository _gridFloorRepository { get; private set; }

    public CharacterModel()
    {
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

        return new Vector2(_gridFloorRepository.Get(targetCellIndex).transform.localPosition.x, Y_WORD_POSITION);
    }
}
