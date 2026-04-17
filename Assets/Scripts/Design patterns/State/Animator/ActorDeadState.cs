using UnityEngine;

public class ActorDeadState : IState
{
    public void Enter()
    {
        Debug.Log("Enter Dead State");
    }

    public void Execute()
    {
        Debug.Log("Executing Dead  State");

    }

    public void Exit()
    {
        Debug.Log("Exit Dead State");
    }
}