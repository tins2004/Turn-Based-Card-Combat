using System;
using UnityEngine;

public class CharacterPresenter : BaseActorPresenter
{
    [SerializeField] private CharacterSO characterData;
    
    [Header("Grid Settings")]
    [SerializeField] private int spawnPos = 3;

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
        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth, _model.currentShield);
        MoveToCell(spawnPos);

        SetupObserverListener();
    }

    public override ScriptableObject GetActorData()
    {
        return _model.actorData as CharacterSO;
    }

    public override void MoveToCell(int targetCell)
    {
        if (targetCell < 0) return;

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
        bool haveHealth = _model.TakeDamage(damage);

        _view.TakeDamageAnimation();
        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth, _model.currentShield);

        if (!haveHealth)
        {
            Observer.Notify(ObserverEvents.PLAYER_DEAD, true);
        }
    }

    public override void AddShield(int shield)
    {
        _model.AddShield(shield);

        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth, _model.currentShield);
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

    private void HandlePlayerRevived(object obj)
    {
        if (!(bool)obj) return;

        _model.currentHealth = _model.maxHealth;
        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth, _model.currentShield);
    }

    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.ACTOR_USED_SKILL, HandleUsedCard);
        Observer.AddListener(ObserverEvents.PLAYER_REVIVED, HandlePlayerRevived);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.ACTOR_USED_SKILL, HandleUsedCard);
        Observer.RemoveListener(ObserverEvents.PLAYER_REVIVED, HandlePlayerRevived);
    }
}
