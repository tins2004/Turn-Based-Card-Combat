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
            "MOVE_WALK",
            "ATTACK_BASE_ATTACK",
            "MOVE_TELEPORT",
            "MOVE_TELEPORT",
            "MOVE_WALK",
            "MOVE_WALK",
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
        DrawVisualCards();
    }

    public void EndTurn()
    {
        _model.EndTurn();
        _view.ClearCardsHand(handParent);
    }

    private void DrawVisualCards()
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
        GameObject usedCard = _model.UsedCard(data.ToString());

        _view.DestroyCardHand(usedCard);

        int childCount = 0;
        foreach (RectTransform child in handParent)
        {
            if (child.gameObject.activeSelf) 
            {
                _view.ArrangeCardsHand(_model.GetCardVisualTransform(childCount), child);
                childCount++;
            }
        }
    }
    
    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.CHOOSE_CARD, HandleUsedCard);
        // Observer.AddListener(ObserverEvents.PLAYER_END_TURN, HandleEndTurn);
        // Observer.AddListener(ObserverEvents.ENEMY_END_TURN, HandleStartTurn);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.CHOOSE_CARD, HandleUsedCard);
        // Observer.RemoveListener(ObserverEvents.PLAYER_END_TURN, HandleEndTurn);
        // Observer.RemoveListener(ObserverEvents.ENEMY_END_TURN, HandleStartTurn);
    }
}
