using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyPresenter))]
public class EnemyView : MonoBehaviour
{
    [SerializeField] private Image healthBox;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text nextActionText;

    private Transform _transform;
    private StateManager _animStateManager;
    private ActorSpriteAnimator _animator;

    private ActorIdleState idleState;
    private ActorAttackState attackState;
    private ActorTakeDamageState takeDamageState;
    private ActorDeadState deadState;
    private ActorInitState initState;

    private void Awake()
    {
        _transform = transform;
        _animStateManager = GetComponent<StateManager>();
        _animator = GetComponent<ActorSpriteAnimator>();

        idleState = new ActorIdleState(_animator);
        attackState = new ActorAttackState(_animator, _animStateManager);
        takeDamageState = new ActorTakeDamageState(_animator, _animStateManager);
        deadState = new ActorDeadState(_animator, gameObject);
        initState = new ActorInitState(_animator, _animStateManager);
    }

    public void ChangePosition(Vector2? targetPos)
    {
        if (targetPos == null) 
            return;
        
        _transform.localPosition = new Vector2(targetPos.Value.x, targetPos.Value.y);
    }

    public void UpdateHealthUI(int currentHeart, int maxHeart, int shield)
    {
        healthBox.color = shield > 0 ? Color.darkBlue : Color.darkRed;
        string shieldText = shield > 0 ? $" [{shield}]" : "";
        healthText.text = $"{currentHeart}/{maxHeart}{shieldText}";
    }

    public void UpdateNextActionUI(string nextActionName)
    {
        nextActionText.text = nextActionName;
    }

    public void UpdateFlipSprite(int currentCell, int targetCell)
    {
        _animator.LookAtCell(currentCell, targetCell);
    }

    public void InitAnimation(IHasAnimations actorData)
    {
        initState.SetActorData(actorData);
        _animStateManager.ChangeSate(initState);
        _animStateManager.ExecuteCurrentState();
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

    public void DeadAnimation()
    {
        _animStateManager.ChangeSate(deadState);
        _animStateManager.ExecuteCurrentState();
    }
}
