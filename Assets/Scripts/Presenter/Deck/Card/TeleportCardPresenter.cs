using System.Collections.Generic;

public class TeleportCardPresenter : CardPresenter
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        rangeCardImpact = 0;
    }

    public override List<int> GetCellsCanImpact()
    {
        List<int> results = new List<int>();

        int currentCell = _model._actorOnFloorRepository.GetCellOfActorType(1)[0];
        int limitFloor = GridFloorRepository.Instance.GetTotalCells() - 1;

        for (int i = 0; i <= limitFloor; i++)
        {
            if (i == currentCell) continue;

            results.Add(i);
        }

        return results;
    }

    public override List<int> GetCellsOnLineImpact(int cellTarget)
    {
        return null;
    }
}
