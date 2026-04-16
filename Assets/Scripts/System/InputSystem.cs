using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : SingletonMonoBehaviour<InputSystem>
{
    private Camera _mainCamera;

    public static event Action<Vector2> OnPointerDown;
    public static event Action<Vector2> OnPointerDrag;
    public static event Action<Vector2> OnPointerUp;

    private bool _isDragging;

    protected override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(mousePos);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isDragging = true;
            OnPointerDown?.Invoke(mousePos);
        }

        if (_isDragging)
        {
            OnPointerDrag?.Invoke(worldPos);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _isDragging = false;
            OnPointerUp?.Invoke(worldPos);
        }

        // if (Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
        //     // Observer.Notify(ObserverEvents.CHARACTER_TAKE_DAMAGE, 10);
        //     Observer.Notify(ObserverEvents.ENEMY_TAKE_DAMAGE, 10);
        // }
    }

    public static RaycastHit2D GetHitUnderPosition(Vector2 worldPos, LayerMask mask)
    {
        return Physics2D.Raycast(worldPos, Vector2.zero, 0f, mask);
    }
}
