using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeckPresenter : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int numberHandCards = 4;

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

}
