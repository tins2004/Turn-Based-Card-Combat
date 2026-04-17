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
        _enemySystem.ExecuteEnemyTurn();

    }

    public void Exit()
    {
        // Cho nó tính toán lại tạng thái cho lược tiếp theo và show ra.
    }
}