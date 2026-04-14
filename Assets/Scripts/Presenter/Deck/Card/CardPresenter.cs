using System.Collections.Generic;
using UnityEngine;

public abstract class CardPresenter : MonoBehaviour
{
    [SerializeField] private CardSO cardData;
    public int rangeCardImpact { get; protected set; }

    protected CardView _view { get; private set; }
    protected CardModel _model { get; private set; }

    protected virtual void Awake()
    {
        if (_view == null)
        {
            _view = GetComponent<CardView>();
        }
    }

    protected virtual void Start()
    {
        _model = new CardModel(cardData);
    }

    public void SelectedCard(bool isSelected)
    {
        _view.UpdateCardVisual(isSelected);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Array Cells allow the Card may have an impact</returns>
    public abstract List<int> GetCellsCanImpact();

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Array Cells actor or subject need to pass through</returns>
    public abstract List<int> GetCellsOnLineImpact(int cellTarget);
}
