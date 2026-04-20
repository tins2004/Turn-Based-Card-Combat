using UnityEngine;

public class LoseState : IState
{
    private RunSystem _runSystem;
    private DeckPresenter _deckPresenter;
    
    public LoseState(RunSystem runSystem)
    {
        _runSystem = runSystem;
    }

    public void Enter()
    {
        if (_deckPresenter == null)
        {
            _deckPresenter = GameObject.FindGameObjectWithTag("DeckController").GetComponent<DeckPresenter>();
        }

        // Debug.Log("Enter Thua");
    }

    public void Execute()
    {
        // Debug.Log("Execute Thua");
        _runSystem.ShowEndPanel("Thua", "Hồi đầy máu và thử lại.");
        Time.timeScale = 0f; 
    }

    public void Exit()
    {
        // Debug.Log("Exit Thua");

        Time.timeScale = 1f; 
        _runSystem.HideEndPanel();

        Observer.Notify(ObserverEvents.PLAYER_REVIVED, true);
        _deckPresenter.EndTurn();
    }
}