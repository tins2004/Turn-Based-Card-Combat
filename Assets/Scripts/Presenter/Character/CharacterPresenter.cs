using UnityEngine;

public class CharacterPresenter : BaseActorPresenter
{
    [SerializeField] private CharacterSO characterData;
    
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
        _model = new CharacterModel(characterData);

        _view.IdleAnimation(characterData);
        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth);
        MoveToCell(spawnPos);

        SetupObserverListener();
    }

    public override ScriptableObject GetActorData()
    {
        return _model.actorData as CharacterSO;
    }

    public override void MoveToCell(int targetCell)
    {
        _view.UpdateFlipSprite(_model.currentActorCell, targetCell);
        base.MoveToCell(targetCell);

        _view.ChangePosition(_model.GetTargetPosition(targetCell));
    }

    public override void Attack(int targetCell, int damage)
    {
        base.Attack(targetCell, damage);

        _view.UpdateFlipSprite(_model.currentActorCell, targetCell);
        _view.AttackAnimation();
    }

    public override void TakeDamage(int damage)
    {
        _model.TakeDamage(damage);

        _view.TakeDamageAnimation();
        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth);
    }

    private void HandleUsedCard(object data)
    {
        if (data is CardActionORD cardAction)
        {
            cardAction.skillStrategy.Execute(this, cardAction.cellTarget, cardAction.skillStrategy.GetSkillData());
        }
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
