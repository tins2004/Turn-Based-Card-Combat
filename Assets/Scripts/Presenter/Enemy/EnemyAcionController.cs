using System.Collections.Generic;
using UnityEngine;

public class EnemyActionController
{
    private EnemyPresenter _owner;
    private SkillCommand _queuedCommand;
    private SkillStrategy skillStrategy;
    private SkillSO plannedSkill;

    private List<int> expectedImpactCells = new List<int>();
    private List<int> cellsHover = new List<int>();
    
    private GridFloorRepository _gridFloorRepository;

    public EnemyActionController(EnemyPresenter owner)
    {
        _owner = owner;

        _gridFloorRepository = GridFloorRepository.Instance;
    }

    public void PlanNextAction()
    {
        expectedImpactCells.Clear();
        _queuedCommand = null;

        // float randomValue = Random.value;

        // if (randomValue < 0.7f) 
            plannedSkill = _owner.GetAttackData();
        // else 
        //     ExecuteSkill(enemy, new BaseBuffStrategy(), currentCell);
        
        skillStrategy = plannedSkill.SkillAlgorithm;
        skillStrategy.ConfigDataSKill(plannedSkill);

        _owner.DisplayNextAction(plannedSkill);
    }

    private void PlanMoveAction(int currentCell, int playerCell)
    {
        plannedSkill = _owner.GetMoveData();
        skillStrategy = plannedSkill.SkillAlgorithm;
        skillStrategy.ConfigDataSKill(plannedSkill);

        List<int> moveCells = skillStrategy.GetRealCellsImpact(currentCell, 2);
        if (moveCells != null && moveCells.Count > 0)
        {
            int bestMoveCell = moveCells[0];
            int minDistance = int.MaxValue;

            foreach (int cellIndex in moveCells)
            {
                var cellsCanImpacts = skillStrategy.GetCellsCanImpact(cellIndex);
                foreach (int dest in cellsCanImpacts)
                {
                    int dist = Mathf.Abs(dest - playerCell);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        bestMoveCell = dest;
                    }
                }
            }

            _queuedCommand = new SkillCommand(skillStrategy, _owner, bestMoveCell, plannedSkill);
        }
    }

    public SkillCommand GetValidCommand(int currentCell, int playerCell)
    {
        if (skillStrategy == null) return null;
        
        expectedImpactCells.Clear();

        if (plannedSkill != _owner.GetMoveData())
        {
            bool isStillValid = false;

            var realImpactCells = skillStrategy.GetRealCellsImpact(currentCell, 2);

            if (realImpactCells != null && realImpactCells.Count > 0)
            {
                foreach (int cellIndex in realImpactCells)
                {
                    var cellsCanImpacts = skillStrategy.GetCellsCanImpact(cellIndex);
                    expectedImpactCells.AddRange(cellsCanImpacts);
                }

                
                if (expectedImpactCells.Count > 0)
                {
                    int randomIndex = Random.Range(0, expectedImpactCells.Count);
                    _queuedCommand = new SkillCommand(skillStrategy, _owner, expectedImpactCells[randomIndex], plannedSkill);
                    
                    isStillValid = true; 
                }
            }


            if (!isStillValid)
            {
                PlanMoveAction(currentCell, playerCell);
            }
        }

        return _queuedCommand;
    }

    public void UpdateRangeSkillVisuals(int currentCell)
    {
        var realImpactCells = skillStrategy.GetRealCellsImpact(currentCell, 2);
        if (realImpactCells != null && realImpactCells.Count > 0)
        {
            CellVisualHighlighter.HighlightCells(_gridFloorRepository.GetGameObjectsByListIndex(realImpactCells), Color.red);
            cellsHover = realImpactCells;
        }
    }

    public void ResetRangeSkillVisuals()
    {
        CellVisualHighlighter.HighlightCells(_gridFloorRepository.GetGameObjectsByListIndex(cellsHover), Color.white);
        cellsHover = null;
    }
}