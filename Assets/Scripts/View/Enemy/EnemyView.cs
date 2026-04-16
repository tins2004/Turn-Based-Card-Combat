using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyPresenter))]
public class EnemyView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void ChangePosition(Vector2? targetPos)
    {
        if (targetPos == null) 
            return;
        
        _transform.localPosition = new Vector2(targetPos.Value.x, targetPos.Value.y);
    }

    public void UpdateHealthUI(int currentHeart, int maxHeart)
    {
        healthText.text = $"{currentHeart}/{maxHeart}";
    }
}
