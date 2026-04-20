using UnityEngine;

public class ActorDeadState : IState
{
    private ActorSpriteAnimator _animator;
    private GameObject enemy;

    public ActorDeadState(ActorSpriteAnimator animator, GameObject enemy)
    {
        _animator = animator;
        this.enemy = enemy;
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

        _animator.PlayAnim(AnimationType.Dead, 
                            loop: false, 
                            onComplete: OnDeadFinished);
    }

    private void OnDeadFinished()
    {
        Observer.Notify(ObserverEvents.ENEMY_DEAD, enemy);
    }


    public void Exit()
    {
        // Debug.Log("Exit Dead State");
    }
}