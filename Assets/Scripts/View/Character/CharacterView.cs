using UnityEngine;

[RequireComponent(typeof(CharacterPresenter))]
public class CharacterView : MonoBehaviour
{
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    public void ChangePosition(Vector2? targetPos)
    {
        if (targetPos == null) 
            return;
        
        _transform.localPosition = new Vector2(targetPos.Value.x, targetPos.Value.y);
    }
}
