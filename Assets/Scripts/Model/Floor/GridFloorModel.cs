using UnityEngine;

public class GridFloorModel
{
    public int width { get; private set; }
    public float cellWidth { get; private set; }
    public float cellHeight { get; private set; }
    public string cellName { get; private set; }
    private const float GAP = 0.05f;

    public GridFloorModel(int width, float cellWidth, float cellHeight)
    {
        this.width = width;
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;
        cellName = "Cell Floor";
    }

    public Vector2 GetCenterOffset()
    {
        float totalWidth = (width * cellWidth) + ((width - 1) * GAP);
        
        return new Vector2(totalWidth / 2f - (cellWidth / 2f), 0);
    }

    public Vector2 GetCellPosition(int x)
    {
        // float xSkewed = y * 0.5f; 
        float posX = x * (cellWidth + GAP);
        return new Vector2(posX, 0) - GetCenterOffset();
    }
}
