using UnityEngine;

public class EnemyPresenter : BaseActorPresenter
{    
    private EnemyView _view;

    private void Awake()
    {
        if (_view == null)
        {
            _view = GetComponent<EnemyView>();
        }
    }

    public void SetUpEnemy(EnemySO enemyData)
    {
        // SetupObserverListener();
        
        _model = new EnemyModel(enemyData);

        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth);
    }

    public override void MoveToCell(int targetCell)
    {
        base.MoveToCell(targetCell);

        _view.ChangePosition(_model.GetTargetPosition(targetCell));
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

    // private void SetupObserverListener()
    // {
    //     Observer.AddListener(ObserverEvents.ENEMY_TAKE_DAMAGE, HandleTakeDamage);
    // }

    // private void OnDestroy()
    // {
    //     Observer.RemoveListener(ObserverEvents.ENEMY_TAKE_DAMAGE, HandleTakeDamage);
    // }
}
