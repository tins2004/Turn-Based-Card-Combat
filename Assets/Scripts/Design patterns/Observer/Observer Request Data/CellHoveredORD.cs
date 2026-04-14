using UnityEngine;

public struct CellHoveredORD
{
    public GameObject CellObject;
    public Color ColorCell;

    public CellHoveredORD(GameObject cell, Color colorCell)
    {
        CellObject = cell;
        ColorCell = colorCell;
    }
}