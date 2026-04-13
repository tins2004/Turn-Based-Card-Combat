using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeckPresenter : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int numberHandCards = 4;

    [Header("Hover Settings")]
    [SerializeField] private LayerMask cardLayer;

    private DeckView _view;
    private DeckModel _model;

    private GameObject currentInteractedCard;

    private void Awake()
    {
        if (_view == null)
        {
            _view = GetComponent<DeckView>();
        }
    }

    private void Start()
    {
        _model = new DeckModel(numberHandCards);
        GenerateHandCards();
    }

    private void GenerateHandCards()
    {

        for (int x = 0; x < _model.numberHandCards; x++)
        {
            _view.SpawnDeck(_model.GetHandCardPosition(x), $"{_model.handCardName} {x}");
        }
    }

    private void HandleInteraction(bool isPressed)
    {
        if (isPressed)
        {
            RaycastHit2D hit = _view.GetHitUnderMouse(cardLayer);

            if (hit.collider != null)
            {   
                currentInteractedCard = hit.collider.gameObject;
                _view.UpdateCellVisual(hit.collider.gameObject, true);

                Observer.Notify(ObserverEvents.SELECTED_CARD, true);
            }
        }
        else
        {
            if (currentInteractedCard != null)
            {
                _view.UpdateCellVisual(currentInteractedCard, false);
                currentInteractedCard = null;

                Observer.Notify(ObserverEvents.SELECTED_CARD, false);
            }
        }        
    }

    private void OnEnable()
    {
        _view.OnMouseInteraction += HandleInteraction;
    }

    private void OnDisable()
    {
        _view.OnMouseInteraction -= HandleInteraction;
    }
}
