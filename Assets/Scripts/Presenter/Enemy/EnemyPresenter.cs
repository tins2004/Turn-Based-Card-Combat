using System.Collections.Generic;
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
        _model = new EnemyModel(enemyData);

        _view.IdleAnimation(enemyData);
        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth);
    }

    public override ScriptableObject GetActorData()
    {
        return _model.actorData as CharacterSO;
    }

    public int GetCurrentCell()
    {
        return _model.currentActorCell;
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
        _view.UpdateFlipSprite(_model.currentActorCell, _model._actorOnFloorRepository.GetCellOfActorType(1)[0]);
        _view.UpdateHealthUI(_model.currentHealth, _model.maxHealth);
    }

    public void DisplayNextAction(SkillSO skillSO)
    {
        _view.UpdateNextActionUI(skillSO.Name);
    }

    #region(Demo AI)
    public SkillSO GetMoveData()
    {
        EnemyModel enemyModel = _model as EnemyModel;
        if (enemyModel != null && enemyModel.enemyData.EnemySkill.Count > 0)
        {
            return enemyModel.enemyData.EnemySkill[0];
        }
        return null;
    }

    public SkillSO GetAttackData()
    {
        EnemyModel enemyModel = _model as EnemyModel;
        if (enemyModel != null && enemyModel.enemyData.EnemySkill.Count > 1)
        {
            return enemyModel.enemyData.EnemySkill[1];
        }
        return null;
    }

    public SkillSO GetThirdSkillData()
    {
        EnemyModel enemyModel = _model as EnemyModel;
        if (enemyModel != null && enemyModel.enemyData.EnemySkill.Count > 2)
        {
            return enemyModel.enemyData.EnemySkill[2];
        }
        return null;
    }
    #endregion

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
