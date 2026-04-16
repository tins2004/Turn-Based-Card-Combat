using System.Collections.Generic;
using UnityEngine;

public class CardPresenter : MonoBehaviour
{
    private CardView _view;
    private CardModel _model;
    public SkillStrategy _skillStrategy { get; private set; }

    private void Awake()
    {
        if (_view == null)
        {
            _view = GetComponent<CardView>();
        }
    }

    public void SetUpCard(CardSO cardData)
    {
        _model = new CardModel(cardData);
        _skillStrategy = cardData.Detail.SkillAlgorithm;
        
        _skillStrategy.ConfigDataSKill(cardData.Detail);

        _view.UpdateCardVisual(false);
        _view.DisplayInformationCard(_model.cardData);
    }

    public void SelectedCard(bool isSelected)
    {
        _view.UpdateCardVisual(isSelected);
    }

    public CardSO GetCardData()
    {
        return _model.cardData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Array Cells follow range of card</returns>
    public List<int> GetRealCellsImpact() => _skillStrategy.GetRealCellsImpact();

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Array Cells allow the Card may have an impact</returns>
    public List<int> GetCellsCanImpact(int realCellImpact) => _skillStrategy.GetCellsCanImpact(realCellImpact);

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Array Cells actor or subject need to pass through</returns>
    public List<int> GetCellsOnLineImpact(int cellTarget) => _skillStrategy.GetCellsOnLineImpact(cellTarget);
}
