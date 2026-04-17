using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterPresenter))]
public class CharacterView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private Transform _transform;

    private StateManager _animStateManager;
    private ActorSpriteAnimator _animator;

    private ActorIdleState idleState;
    private ActorAttackState attackState;
    private ActorTakeDamageState takeDamageState;

    private void Start()
    {
        _transform = transform;
        _animStateManager = GetComponent<StateManager>();
        _animator = GetComponent<ActorSpriteAnimator>();

        idleState = new ActorIdleState(_animator);
        attackState = new ActorAttackState(_animator, _animStateManager);
        takeDamageState = new ActorTakeDamageState(_animator, _animStateManager);
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
    
    public void IdleAnimation(IHasAnimations actorData)
    {
        idleState.SetActorData(actorData);
        _animStateManager.ChangeSate(idleState);
        _animStateManager.ExecuteCurrentState();
    }

    public void AttackAnimation()
    {
        _animStateManager.ChangeSate(attackState);
        _animStateManager.ExecuteCurrentState();
    }

    public void TakeDamageAnimation()
    {
        _animStateManager.ChangeSate(takeDamageState);
        _animStateManager.ExecuteCurrentState();
    }
}
