using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : SingletonMonoBehaviour<EnemySystem>
{
    [SerializeField] private LayerMask floorLayer;

    private string enemyObjectName = "Enemy";

    private CommandEnemyInvoker _commandEnemyInvoker;
    private Dictionary<EnemyPresenter, EnemyActionController> enemyControllers = new Dictionary<EnemyPresenter, EnemyActionController>();
    private EnemyPresenter enemyHover;

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
        _commandEnemyInvoker = new CommandEnemyInvoker();

        _actorOnFloorRepository = ActorOnFloorRepository.Instance;

        SpawnEnemies("WARRIOR_AXE", 5);
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

    private void SpawnEnemies(string enemyId, int cellTarget)
    {
        // for (int i = 0; i < _model.hand.Count; i++)
        // {
                CreateEnemy(enemyId, cellTarget, transform, $"{enemyObjectName} {0}");
            // EnemyPresenter card = CreateEnemy(enemyId, cellTarget, transform, $"{enemyObjectName} {0}");
        // }
        EvaluateEnemiesNextAction();
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

    public void RemoveEnemy(EnemyPresenter enemy)
    {
        if (enemyControllers.ContainsKey(enemy))
        {
            enemyControllers.Remove(enemy);
        }
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
        int playerCell = _actorOnFloorRepository.GetCellOfActorType(1)[0];

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
