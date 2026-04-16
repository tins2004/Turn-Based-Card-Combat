using System.Collections.Generic;
using UnityEngine;

public class DeckPresenter : MonoBehaviour
{
    [Header("Hand Card")]
    [SerializeField] private int cardsPerTurn = 5;
    [SerializeField] private Transform handParent;

    private DeckView _view;
    private DeckModel _model;

    private void Awake()
    {
        if (_view == null)
        {
            _view = GetComponent<DeckView>();
        }
    }

    private void Start()
    {
        SetupObserverListener();

        List<string> startingCards = new List<string> { 
            "MOVE_DASH",
            "ATTACK_BASE_ATTACK",
            "MOVE_TELEPORT",
            "MOVE_TELEPORT",
            "MOVE_TELEPORT",
            "MOVE_TELEPORT",
            "ATTACK_BASE_ATTACK",
            "ATTACK_BASE_ATTACK",
            "ATTACK_BASE_ATTACK",
            "MOVE_DASH",
            "MOVE_DASH"
        };

        _model = new DeckModel(startingCards);
        StartTurn();

    }

    public void StartTurn()
    {
        _model.DrawCards(cardsPerTurn);
        UpdateHandUI();
    }

    private void UpdateHandUI()
    {
        _view.ClearCardsHand(handParent);

        for (int i = 0; i < _model.hand.Count; i++)
        {
            GameObject card = _view.DrawCardsHand(_model.hand[i], _model.GetCardVisualTransform(i), handParent, $"{_model.handCardName} {i}");

            _model._handCardsRepository.Add(card.gameObject.name, card.GetComponent<CardPresenter>());
        }

        
        _view.DisplayDeckDemo(_model.drawPile.Count, string.Join("\n", _model.drawPile), _model.discardPile.Count);
    }

    public void HandleUsedCard(object data)
    {
        _model.UsedCard(data.ToString());
        UpdateHandUI();
    }

    public void HandleEndTurn(object data)
    {
        if (!(bool)data) return;

        _model.EndTurn();
        _view.ClearCardsHand(handParent);

        StartTurn();
        UpdateHandUI();
    }
    
    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.CHOOSE_CARD, HandleUsedCard);
        Observer.AddListener(ObserverEvents.END_TURN, HandleEndTurn);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.CHOOSE_CARD, HandleUsedCard);
        Observer.RemoveListener(ObserverEvents.END_TURN, HandleEndTurn);
    }
}
