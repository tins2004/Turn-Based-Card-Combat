using UnityEngine;

public class ActorInitState : IState
{
    private ActorSpriteAnimator _animator;
    private StateManager _actorMachine;

    public ActorInitState(ActorSpriteAnimator animator, StateManager actorMachine)
    {
        _animator = animator;
        _actorMachine = actorMachine;
    }

    public void SetActorData(IHasAnimations actorData)
    {
        if (actorData == null)
        {
            Debug.LogWarning("Actor data is null in ActorIdleState");
            return;
        }

        _animator.SetActorData(actorData);
    }

    public void Enter()
    {
        // Debug.Log("Enter Dead State");
    }

    public void Execute()
    {
        if (_animator == null)
        {
            Debug.LogError("Animator is null in ActorDeadState");
            return;
        }

        _animator.PlayAnim(AnimationType.Init, 
                            loop: false, 
                            onComplete: OnInitFinished);
    }

    private void OnInitFinished()
    {
        _actorMachine.ChangeSate(new ActorIdleState(_animator));
        _actorMachine.ExecuteCurrentState();
    }


    public void Exit()
    {
        // Debug.Log("Exit Dead State");
    }
}