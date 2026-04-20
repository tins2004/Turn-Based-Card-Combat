using UnityEngine;

public class WinState : IState
{
    private RunSystem _runSystem;
    
    public WinState(RunSystem runSystem)
    {
        _runSystem = runSystem;
    }

    public void Enter() { }

    public void Execute()
    {
        // _runSystem.ShowEndPanel("")
    }

    public void Exit()
    {
        // _deckPresenter.EndTurn();
    }
}