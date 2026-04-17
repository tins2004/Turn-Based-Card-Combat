using UnityEngine;

public class ActorAttackState : IState
{
    private ActorSpriteAnimator _animator;
    private StateManager _actorMachine;

    public ActorAttackState(ActorSpriteAnimator animator, StateManager actorMachine)
    {
        _animator = animator;
        _actorMachine = actorMachine;
    }

    public void Enter()
    {
        // Debug.Log("Enter Attack State");
    }

    public void Execute()
    {
        if (_animator == null)
        {
            Debug.LogError("Animator is null in ActorAttackState");
            return;
        }

        _animator.PlayAnim(AnimationType.Attack, 
                            loop: false, 
                            onComplete: OnAttackFinished);
    }

    private void OnAttackFinished()
    {
        _actorMachine.ChangeSate(new ActorIdleState(_animator));
        _actorMachine.ExecuteCurrentState();
    }

    public void Exit()
    {
        // Debug.Log("Exit Attack State");
    }
}