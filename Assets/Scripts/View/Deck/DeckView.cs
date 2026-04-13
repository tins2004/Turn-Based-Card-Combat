using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DeckPresenter))]
public class DeckView : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    private Camera _mainCamera;

    public event Action<bool> OnMouseInteraction;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void SpawnDeck(Vector2 position, string name)
    {
        GameObject obj = Instantiate(cardPrefab, transform);

        obj.transform.localPosition = position;

        obj.name = name;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnMouseInteraction?.Invoke(true);

            
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            OnMouseInteraction?.Invoke(false);
        }
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
