using UnityEngine;

public class ActorTakeDamageState : IState
{
    private ActorSpriteAnimator _animator;
    private StateManager _actorMachine;

    public ActorTakeDamageState(ActorSpriteAnimator animator, StateManager actorMachine)
    {
        _animator = animator;
        _actorMachine = actorMachine;
    }

    public void Enter()
    {
    }

    public void Execute()
    {
        if (_animator == null)
        {
            Debug.LogError("Animator is null in ActorAttackState");
            return;
        }

        _animator.TakeDamageAnim(OnTakeDamageFinished);
    }

    private void OnTakeDamageFinished()
    {
        _actorMachine.ChangeSate(new ActorIdleState(_animator));
        _actorMachine.ExecuteCurrentState();
    }

    public void Exit()
    {
    }
}