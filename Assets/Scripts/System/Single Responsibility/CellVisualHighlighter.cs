using UnityEngine;
using System.Collections.Generic;

public static class CellVisualHighlighter
{
    public static void HighlightCells(IEnumerable<GameObject> cells, Color color, string eventName = ObserverEvents.CELL_HOVERED)
    {
        if (cells == null) return;
        foreach (var cell in cells)
        {
            if (cell != null)
                Observer.Notify(eventName, new CellHoveredORD(cell, color));
        }
    }

    public static void HighlightSingle(GameObject cell, Color color, string eventName = ObserverEvents.CELL_HOVERED)
    {
        if (cell != null)
            Observer.Notify(eventName, new CellHoveredORD(cell, color));
    }
}