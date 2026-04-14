using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DeckPresenter))]
public class DeckView : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;

    public void SpawnDeck(Vector2 position, string name)
    {
        GameObject obj = Instantiate(cardPrefab, transform);

        obj.transform.localPosition = position;

        obj.name = name;
    }
}
