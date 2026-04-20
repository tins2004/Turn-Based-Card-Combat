using System.Collections.Generic;
using UnityEngine;

public class DeckPresenter : MonoBehaviour
{
    [Header("Hand Card")]
    [SerializeField] private int cardsPerTurn = 5;
    [SerializeField] private int startingEnergy = 3;
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
            "MOVE_WALK",
            "MOVE_WALK",
            "MOVE_DASH",
            "MOVE_TELEPORT",
            "ATTACK_BASE_ATTACK",
            "ATTACK_BASE_ATTACK",
            "ATTACK_BASE_ATTACK",
            "ABILITY_DEFEND",
            "ABILITY_DEFEND"
        };

        _model = new DeckModel(startingCards, startingEnergy);
        StartTurn();

    }

    public void StartTurn()
    {
        _model.DrawCards(cardsPerTurn);
        _model.currentEnergy = _model.startingEnergy;
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

        
        _view.DisplayDeckDemo(_model.currentEnergy, _model.drawPile.Count, string.Join("\n", _model.drawPile), _model.discardPile.Count);
    }

    public bool HaveEnergyToUseCard(int cardEnergyRequired)
    {
        if (_model.currentEnergy < cardEnergyRequired)
        {
            return false;
        }

        return true;
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

        _view.DisplayDeckDemo(_model.currentEnergy, _model.drawPile.Count, string.Join("\n", _model.drawPile), _model.discardPile.Count);
    }
    
    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.CARD_USED, HandleUsedCard);
        // Observer.AddListener(ObserverEvents.ENEMY_END_TURN, HandleStartTurn);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.CARD_USED, HandleUsedCard);
        // Observer.RemoveListener(ObserverEvents.ENEMY_END_TURN, HandleStartTurn);
    }
}
