using UnityEngine;

public class PlayerTurnState : IState
{
    private DeckPresenter _deckPresenter;

    public void Enter()
    {
        if (_deckPresenter == null)
        {
            _deckPresenter = GameObject.FindGameObjectWithTag("DeckController").GetComponent<DeckPresenter>();
        }
    }

    public void Execute()
    {
        _deckPresenter.StartTurn();
    }

    public void Exit()
    {
        _deckPresenter.EndTurn();
    }
}