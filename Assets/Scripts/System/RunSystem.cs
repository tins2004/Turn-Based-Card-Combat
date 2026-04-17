using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StateManager))]
public class RunSystem : SingletonMonoBehaviour<RunSystem>
{
    [SerializeField] private Button endTurnButton;
    private StateManager _runStateManager;

    private void Start()
    {
        _runStateManager = GetComponent<StateManager>();

        _runStateManager.ChangeSate(new PlayerTurnState());

        endTurnButton.onClick.AddListener(OnClickEndTurnButton);
    }

    private void OnClickEndTurnButton()
    {
        _runStateManager.ChangeSate(new EnemyTurnState());
        _runStateManager.ExecuteCurrentState();

        _runStateManager.ChangeSate(new PlayerTurnState());
        _runStateManager.ExecuteCurrentState();
    }
}
