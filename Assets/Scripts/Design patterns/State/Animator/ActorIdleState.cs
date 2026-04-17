using UnityEngine;

public class ActorIdleState : IState
{
    private ActorSpriteAnimator _animator;
    public ActorIdleState(ActorSpriteAnimator animator)
    {
        _animator = animator;
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
        // Debug.Log("Enter Idle State");
    }

    public void Execute()
    {
        if (_animator == null)
        {
            Debug.LogError("Animator is null in ActorIdleState");
            return;
        }

        _animator.PlayAnim(AnimationType.Idle);
    }

    public void Exit()
    {
        // Debug.Log("Exit Idle State");
    }
}