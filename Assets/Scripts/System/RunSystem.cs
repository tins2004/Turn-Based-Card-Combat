using System;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StateManager))]
public class RunSystem : SingletonMonoBehaviour<RunSystem>
{
    [SerializeField] private Button endTurnButton;

    [SerializeField] private GameObject endPanel;
    [SerializeField] private TMP_Text tileEndText;
    [SerializeField] private Button endButton;
    private TMP_Text endButtonText;

    private StateManager _runStateManager;
    private EnemyTurnState enemyTurnState;
    private PlayerTurnState playerTurnState;
    private WinState winState;
    private LoseState loseState;

    private void Start()
    {
        SetupObserverListener();
        
        _runStateManager = GetComponent<StateManager>();

        _runStateManager.ChangeSate(new PlayerTurnState());

        endTurnButton.onClick.AddListener(OnClickEndTurnButton);

        endButtonText = endButton.GetComponentInChildren<TMP_Text>();
        endButton.onClick.AddListener(OnClickEndButton);
        HideEndPanel();

        enemyTurnState = new EnemyTurnState();
        playerTurnState = new PlayerTurnState();
        winState = new WinState(this);
        loseState = new LoseState(this);
    }

    private void OnClickEndTurnButton()
    {
        if (_runStateManager.GetCurrentState() == winState || _runStateManager.GetCurrentState() == loseState)
            return;

        _runStateManager.ChangeSate(enemyTurnState);
        _runStateManager.ExecuteCurrentState();

        if (_runStateManager.GetCurrentState() == winState || _runStateManager.GetCurrentState() == loseState)
            return;

        _runStateManager.ChangeSate(playerTurnState);
        _runStateManager.ExecuteCurrentState();
    }

    private void OnClickEndButton()
    {
        _runStateManager.ChangeSate(playerTurnState);
        _runStateManager.ExecuteCurrentState();
    }

    private void HandlePlayerDead(object obj)
    {
        if (!(bool)obj) return;

        _runStateManager.ChangeSate(loseState);
        _runStateManager.ExecuteCurrentState();
    }

    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.PLAYER_DEAD, HandlePlayerDead);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.ACTOR_USED_SKILL, HandlePlayerDead);
    }

    public void ShowEndPanel(string title, string buttonText)
    {
        tileEndText.text = title;
        endButtonText.text = buttonText;
        
        endPanel.SetActive(true);
    }

    public void HideEndPanel()
    {
        if (endPanel != null) endPanel.SetActive(false);
    }
}
