using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GridFloorPresenter))]
public class GridFloorView : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    private Camera _mainCamera;

    public event Action OnMouseInteraction;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void SpawnCell(Vector2 position, Vector3 scale, string name)
    {
        GameObject obj = Instantiate(cellPrefab, transform);

        obj.transform.localPosition = position;
        obj.transform.GetChild(0).localScale = scale;

        obj.name = name;
    }

    private void Update()
    {
        OnMouseInteraction?.Invoke();
    }

    public RaycastHit2D GetHitUnderMouse(LayerMask mask) 
    {
        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return Physics2D.Raycast(worldPos, Vector2.zero, 0f, mask);
    }

    public void UpdateCellVisual(GameObject cell, bool isHovered) 
    {
        var spriteRenderer = cell.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null) 
        {
            Color color = spriteRenderer.color;
            color.a = isHovered ? 1f : 0.2f;
            spriteRenderer.color = color;
        }
    }
}
