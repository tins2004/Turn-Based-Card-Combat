using UnityEngine;

[RequireComponent(typeof(GridFloorPresenter))]
public class GridFloorView : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    public GameObject SpawnCell(Vector2 position, Vector3 scale, string name)
    {
        GameObject obj = Instantiate(cellPrefab, transform);

        obj.transform.localPosition = position;
        obj.transform.GetChild(0).localScale = scale;

        obj.name = name;

        return obj;
    }

    /// <summary>
    /// 
    /// </summary>
    public void UpdateCellVisual(GameObject cell, Color newColor) 
    {
        var spriteRenderer = cell.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null) 
        {
            Color color = newColor;
    
            color.a = (newColor != Color.white) ? 0.3f : 0f;
            spriteRenderer.color = color;
        }
    }
}
