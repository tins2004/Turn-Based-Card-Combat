using UnityEngine;

public class EnemyTurnState : IState
{
    private EnemySystem _enemySystem;

    public void Enter()
    {
        if (_enemySystem == null)
        {
            _enemySystem = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemySystem>();
        }
    }

    public void Execute()
    {
        _enemySystem.PrepareAndExecuteActions();
    }

    public void Exit()
    {
        _enemySystem.EvaluateEnemiesNextAction();
    }
}