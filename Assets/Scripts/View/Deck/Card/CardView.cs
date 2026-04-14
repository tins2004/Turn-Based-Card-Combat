using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardView : MonoBehaviour
{
    public void UpdateCardVisual(bool isSelected) 
    {
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null) 
        {
            Color color = spriteRenderer.color;
            color.a = isSelected ? 1f : 0.2f;
            spriteRenderer.color = color;
        }
    }
}
