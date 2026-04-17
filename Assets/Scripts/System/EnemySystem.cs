using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : SingletonMonoBehaviour<EnemySystem>
{
    private string enemyObjectName = "Enemy";
    private ActorOnFloorRepository _actorOnFloorRepository;

    private void Start()
    {
        // SetupObserverListener();

        // List<string> startingEnemy = new List<string> { 
        //     "MOVE_DASH",
        //     "MOVE_DASH",
        //     "MOVE_TELEPORT",
        //     "MOVE_TELEPORT",
        //     "MOVE_TELEPORT",
        //     "MOVE_TELEPORT",
        //     "MOVE_DASH",
        //     "MOVE_DASH"
        // };

        // _model = new DeckModel(startingCards);
        _actorOnFloorRepository = ActorOnFloorRepository.Instance;

        SpawnEnemies("WARRIOR_AXE", 5);
    }

    private void SpawnEnemies(string enemyId, int cellTarget)
    {
        // for (int i = 0; i < _model.hand.Count; i++)
        // {
                CreateEnemy(enemyId, cellTarget, transform, $"{enemyObjectName} {0}");
            // EnemyPresenter card = CreateEnemy(enemyId, cellTarget, transform, $"{enemyObjectName} {0}");
        // }
    }

    public EnemyPresenter CreateEnemy(string enemyId, int cellTarget, Transform enemyParent, string name)
    {
        EnemyPresenter enemyPresenter = EnemyFactory.Instance.CreateEnemy(enemyId, enemyParent);

        if (enemyPresenter != null)
        {
            enemyPresenter.MoveToCell(cellTarget);
            enemyPresenter.name = name;

            return enemyPresenter;
        }

        return null;
    }

    #region(Demo AI)
    public void ExecuteEnemyTurn()
    {
        int playerCell = _actorOnFloorRepository.GetCellOfActorType(1)[0];
        int[] enemyCells = _actorOnFloorRepository.GetCellOfActorType(2);

        foreach (int cell in enemyCells)
        {
            BaseActorPresenter actor = _actorOnFloorRepository.GetActorObject(cell);
            if (actor is EnemyPresenter enemy)
            {
                DecideEnemyAction(enemy, cell, playerCell);
            }
        }
    }

    private void DecideEnemyAction(EnemyPresenter enemy, int currentCell, int playerCell)
    {
        int distanceToPlayer = Mathf.Abs(currentCell - playerCell);
        
        int attackRange = enemy.GetAttackData().RangeSkillImpact; 
        
        float randomValue = Random.value; // Trả về 0.0 -> 1.0

        if (distanceToPlayer <= attackRange)
        {

            ExecuteSkill(enemy, currentCell, enemy.GetAttackData());

            // TRONG TẦM ĐÁNH: 70% Attack, 30% Buff
            // if (randomValue < 0.7f) 
            //     ExecuteSkill(enemy, new BaseAttackStrategy(), playerCell);
            // else 
            //     ExecuteSkill(enemy, new BaseBuffStrategy(), currentCell);
        }
        else
        {
            ExecuteSkill(enemy, currentCell, enemy.GetMoveData());

            // NGOÀI TẦM ĐÁNH: 70% Di chuyển, 30% Buff
            // if (randomValue < 0.7f)
                // ExecuteSkill(enemy, new DashSkillStrategy(), playerCell);
            // else
            //     ExecuteSkill(enemy, new BaseBuffStrategy(), currentCell);
        }
    }

    private void ExecuteSkill(EnemyPresenter enemy, int currentCell, SkillSO skillData)
    {
        SkillStrategy strategy = skillData.SkillAlgorithm;
        if (strategy == null)
        {
            Debug.LogError($"No strategy found for skill {skillData.name}");
            return;
        }

        strategy.ConfigDataSKill(skillData);

        List<int> realCellsImpact = strategy.GetRealCellsImpact(currentCell,2);

        if (realCellsImpact != null && realCellsImpact.Count > 0)
        {
            strategy.Execute(enemy, strategy.GetCellsCanImpact(realCellsImpact[0])[0], skillData);
        }
    }
    #endregion
}
