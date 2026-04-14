using System.Collections.Generic;

public class DashCardPresenter : CardPresenter
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        rangeCardImpact = 2;
    }

    public override List<int> GetCellsCanImpact()
    {
        List<int> results = new List<int>();

        int currentCell = _model._actorOnFloorRepository.GetCellOfActorType(1)[0];
        int limitFloor = GridFloorRepository.Instance.GetTotalCells() - 1;

        if ((currentCell - rangeCardImpact) < 0)
        {
            if (currentCell != 0)
            {
                results.Add(0);
            }
        }
        else
        {
            results.Add(currentCell - rangeCardImpact);
        }

        if ((currentCell + rangeCardImpact) > limitFloor)
        {
            if (currentCell != limitFloor)
            {
                results.Add(limitFloor);
            }
        }
        else
        {
            results.Add(currentCell + rangeCardImpact);
        }

        return results;
    }

    public override List<int> GetCellsOnLineImpact(int cellTarget)
    {
        List<int> results = new List<int>();

        int currentCell = _model._actorOnFloorRepository.GetCellOfActorType(1)[0];

        int direction = (cellTarget > currentCell) ? 1 : -1;
        
        int backCell = cellTarget - direction;
        
        while (true)
        {
            results.Add(backCell);
            
            if (backCell == currentCell) break;
            backCell -= direction;
        }
        
        return results;
    }
}
