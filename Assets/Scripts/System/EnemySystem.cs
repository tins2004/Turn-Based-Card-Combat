using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : SingletonMonoBehaviour<EnemySystem>
{
    [SerializeField] private LayerMask floorLayer;

    private string enemyObjectName = "Enemy";

    private CommandEnemyInvoker _commandEnemyInvoker;
    private Dictionary<EnemyPresenter, EnemyActionController> enemyControllers = new Dictionary<EnemyPresenter, EnemyActionController>();
    private EnemyPresenter enemyHover;

    private EnemiesRepository _enemiesRepository;
    private ActorOnFloorRepository _actorOnFloorRepository;
    private ObjectPool _enemyPool;

    private void Start()
    {
        SetupObserverListener();

        List<string> startingEnemy = new List<string> { 
            "WARRIOR_AXE",
            "WARRIOR_AXE",
        };

        _commandEnemyInvoker = new CommandEnemyInvoker();

        _enemiesRepository = EnemiesRepository.Instance;
        _actorOnFloorRepository = ActorOnFloorRepository.Instance;
        _enemyPool = GetComponent<ObjectPool>();

        SpawnEnemies(startingEnemy);
    }

    private void OnEnable()
    {
        InputSystem.OnPointerDown += HandlePointerDown;
        InputSystem.OnPointerUp += HandlePointerUp;
    }

    private void OnDisable()
    {
        InputSystem.OnPointerDown -= HandlePointerDown;
        InputSystem.OnPointerUp -= HandlePointerUp;
    }

    private void HandlePointerDown(Vector2 mousePos, Vector2 worldPos)
    {
        
        RaycastHit2D hit = InputSystem.GetHitUnderPosition(worldPos, floorLayer);

        if (hit.collider != null) 
        {   
            int cellIndex = GetCellIndexFromCellName(hit.collider.name);

            if (_actorOnFloorRepository.GetActorType(cellIndex) == 2)
            {
                enemyHover = _actorOnFloorRepository.GetActorObject(cellIndex) as EnemyPresenter;
                enemyControllers[enemyHover].UpdateRangeSkillVisuals(enemyHover.GetCurrentCell());
            }

        }
    }

    private void HandlePointerUp(Vector2 worldPos)
    {
        if (enemyHover != null)
        {
            enemyControllers[enemyHover].ResetRangeSkillVisuals();
            enemyHover = null;
        }
    }

    private void SpawnEnemies(List<string> startingEnemy)
    {
        int[] spaceCells = _actorOnFloorRepository.GetCellsByActorType(0);

        if (spaceCells.Length < startingEnemy.Count)
        {
            Debug.LogWarning("Not enough slots to spawn all the enemies!!");
        }

        spaceCells = ShuffleCells(spaceCells);

        int spawnCount = Math.Min(startingEnemy.Count, spaceCells.Length);
        for (int i = 0; i < spawnCount; i++)
        {
            EnemyPresenter enemyPresenter = CreateEnemy(startingEnemy[i], spaceCells[i], transform, $"{enemyObjectName} {0}");
            _enemiesRepository.Add(spaceCells[i], enemyPresenter);

        }
        EvaluateEnemiesNextAction();
    }

    private int[] ShuffleCells(int[] cellsArray)
    {
        System.Random rng = new System.Random();
        int n = cellsArray.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = cellsArray[k];
            cellsArray[k] = cellsArray[n];
            cellsArray[n] = value;
        }

        return cellsArray;
    }

    public EnemyPresenter CreateEnemy(string enemyId, int cellTarget, Transform enemyParent, string name)
    {
        EnemyPresenter enemyPresenter = EnemyFactory.Instance.CreateEnemy(enemyId, enemyParent);

        if (enemyPresenter != null)
        {
            enemyPresenter.MoveToCell(cellTarget);
            enemyPresenter.name = name;

            if (!enemyControllers.ContainsKey(enemyPresenter))
            {
                enemyControllers.Add(enemyPresenter, new EnemyActionController(enemyPresenter));
            }

            return enemyPresenter;
        }

        return null;
    }

    public void EvaluateEnemiesNextAction()
    {
        foreach (var pair in enemyControllers)
        {
            int currentCell = pair.Key.GetCurrentCell();
            
            if (currentCell != -1)
            {
                pair.Value.PlanNextAction();
            }
        }
    }

    public void PrepareAndExecuteActions()
    {
        int playerCell = _actorOnFloorRepository.GetCellsByActorType(1)[0];

        foreach (var pair in enemyControllers)
        {
            int currentCell = pair.Key.GetCurrentCell(); 
            if (currentCell == -1) return;

            var validCommand = pair.Value.GetValidCommand(currentCell, playerCell);
            if (validCommand != null)
            {
                _commandEnemyInvoker.Enqueue(validCommand);
            }
        }

        _commandEnemyInvoker.ProcessAll();
    }

    public void RemoveEnemyData(EnemyPresenter enemy)
    {
        if (enemyControllers.ContainsKey(enemy))
        {
            enemyControllers.Remove(enemy);
        }

        int enemyCell = _enemiesRepository.GetCellByEnemyPresenter(enemy);
        if (enemyCell != -1)
        {
            _enemiesRepository.Remove(enemyCell);
        }

        int[] allEnemiesCells = _actorOnFloorRepository.GetCellsByActorType(2);
        foreach (int cell in allEnemiesCells)
        {
            if (cell == enemyCell)
            {
                _actorOnFloorRepository.Add(cell, new ActorOnFloorData { actorType = 0, actorObject = null });
            }
                
        }
    }

    private void CheckEnemiesCountAndRespawn()
    {
        int[] enemiesCells = _actorOnFloorRepository.GetCellsByActorType(2);
        if (enemiesCells == null || enemiesCells.Length > 0) return;

        List<string> enemiesSpawn = new List<string>();
        int randomCount = UnityEngine.Random.Range(1, 5);

        for (int i = 0; i < randomCount; i++)
        {
            enemiesSpawn.Add("WARRIOR_AXE");
        }

        SpawnEnemies(enemiesSpawn);
    }

    public void HandleEnemyDead(object data)
    {
        if (data is GameObject enemy)
        {
            if (_enemyPool == null)
            {
                _enemyPool = GetComponent<ObjectPool>();
            }

            RemoveEnemyData(enemy.GetComponent<EnemyPresenter>());
            _enemyPool.ReturnObject(enemy);

            CheckEnemiesCountAndRespawn();
        }
    }

    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.ENEMY_DEAD, HandleEnemyDead);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.ENEMY_DEAD, HandleEnemyDead);
    }

    private int GetCellIndexFromCellName(string cellName)
    {
        if (string.IsNullOrEmpty(cellName)) return 0;
        
        string[] partsName = cellName.Split(' ');
        string lastPart = partsName[partsName.Length - 1];

        if (int.TryParse(lastPart, out int cellIndex)) 
        {
            return cellIndex;
        }

        Debug.LogError("Cell Name is Wrong!");
        return 0;
    }
}
