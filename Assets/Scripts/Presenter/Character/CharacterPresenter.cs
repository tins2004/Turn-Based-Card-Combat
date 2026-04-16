using UnityEngine;

public class CharacterPresenter : BaseActorPresenter
{
    [Header("Grid Settings")]
    [SerializeField] private int spawnPos = 0;

    private CharacterView _view;

    private void Awake()
    {
        if (_view == null)
        {
            _view = GetComponent<CharacterView>();
        }
    }

    private void Start()
    {
        _model = new CharacterModel();

        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth);
        MoveToCell(spawnPos);

        SetupObserverListener();
    }

    public override void MoveToCell(int targetCell)
    {
        base.MoveToCell(targetCell);

        _view.ChangePosition(_model.GetTargetPosition(targetCell));
    }

    private void HandleUsedCard(object data)
    {
        if (data is CardActionORD cardAction)
        {
            cardAction.skillStrategy.Execute(this, cardAction.cellTarget, cardAction.skillStrategy.GetSkillData());
        }
    }

    public override void TakeDamage(int damage)
    {
        _model.TakeDamage(damage);
        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth);
    }

    // private void HandleTakeDamage(object data)
    // {
    //     _model.TakeDamage((int)data);
    //     _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth);
    // }

    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.USED_CARD, HandleUsedCard);
        // Observer.AddListener(ObserverEvents.CHARACTER_TAKE_DAMAGE, HandleTakeDamage);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.USED_CARD, HandleUsedCard);
        // Observer.RemoveListener(ObserverEvents.CHARACTER_TAKE_DAMAGE, HandleTakeDamage);
    }
}
