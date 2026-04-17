using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] IState _currentState;
    public void ChangeSate(IState state)
    {
        if (state == null) return;

        if (_currentState != null && _currentState.GetType() == state.GetType()) return;

        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = state;
        _currentState.Enter();
    }

    public void ExecuteCurrentState()
    {
        if (_currentState != null)
        {
            _currentState.Execute();
        }
    }
}