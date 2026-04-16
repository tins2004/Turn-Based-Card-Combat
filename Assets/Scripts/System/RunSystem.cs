using UnityEngine;
using UnityEngine.UI;

public class RunSystem : SingletonMonoBehaviour<RunSystem>
{
    [SerializeField] private Button endTurnButton;

    private void Start()
    {
        endTurnButton.onClick.AddListener(OnClickEndTurnButton);
    }

    private void OnClickEndTurnButton()
    {
        Observer.Notify(ObserverEvents.END_TURN, true);
    }
}
