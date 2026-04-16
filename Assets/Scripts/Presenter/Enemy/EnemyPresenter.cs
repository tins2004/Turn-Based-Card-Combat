using UnityEngine;

public class EnemyPresenter : MonoBehaviour
{    
    private EnemyView _view;
    private EnemyModel _model;

    private void Awake()
    {
        if (_view == null)
        {
            _view = GetComponent<EnemyView>();
        }
    }

    public void SetUpEnemy(EnemySO enemyData)
    {
        SetupObserverListener();
        
        _model = new EnemyModel(enemyData);

        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth);
    }

    public void MoveToCell(int cell)
    {
        _view.ChangePosition(_model.GetTargetPosition(cell));

        if (_model._actorOnFloorRepository.Exists(_model.currentEnemyCell))
        {
            if (_model._actorOnFloorRepository.Get(_model.currentEnemyCell) == 2)
            {
                _model._actorOnFloorRepository.Add(_model.currentEnemyCell, 0);
            }
        }

        _model.currentEnemyCell = cell;
        _model._actorOnFloorRepository.Add(cell, 2);
    }

    private void HandleTakeDamage(object data)
    {
        _model.TakeDamage((int)data);
        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth);
    }

    private void SetupObserverListener()
    {
        Observer.AddListener(ObserverEvents.ENEMY_TAKE_DAMAGE, HandleTakeDamage);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(ObserverEvents.ENEMY_TAKE_DAMAGE, HandleTakeDamage);
    }
}
