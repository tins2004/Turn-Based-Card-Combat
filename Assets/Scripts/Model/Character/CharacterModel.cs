using UnityEngine;

public class CharacterModel
{
    private const float YWORDPOSITION = 1.15f;
    public int currentPosition { get; private set; }

    public CharacterModel(int currentPosition)
    {
        this.currentPosition = currentPosition;
    }

    public Vector2? GetTargetPosition(int targetCellIndex)
    {
        var repo = GridFloorRepository.Instance;

        if (!repo.Exists(targetCellIndex))
        {
            Debug.LogWarning($"Cell index {targetCellIndex} does not exist in Repository.");
            return null;
        }

        return new Vector2(repo.Get(targetCellIndex), YWORDPOSITION);
    }
}
