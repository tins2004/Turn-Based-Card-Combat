using UnityEngine;

public abstract class BaseActorPresenter : MonoBehaviour 
{
    protected BaseActorModel _model;

    public virtual ScriptableObject GetActorData()
    {
        return _model.actorData;
    }

    public virtual void MoveToCell(int targetCell)
    {
        if (_model._actorOnFloorRepository.Exists(targetCell)) return;

        if (_model._actorOnFloorRepository.Exists(_model.currentActorCell))
        {
            if (_model._actorOnFloorRepository.GetActorType(_model.currentActorCell) == (_model is CharacterModel ? 1 : 2))
            {
                _model._actorOnFloorRepository.Add(_model.currentActorCell, new ActorOnFloorData { actorType = 0, actorObject = null });
            }
        }

        if (_model is EnemyModel)
        {
            EnemiesRepository.Instance.Add(targetCell, this as EnemyPresenter);
            EnemiesRepository.Instance.Remove(_model.currentActorCell);
        }

        _model.currentActorCell = targetCell;
        _model._actorOnFloorRepository.Add(targetCell, new ActorOnFloorData { actorType = _model is CharacterModel ? 1 : 2, actorObject = this });
    }

    public virtual void Attack(int targetCell, int damage)
    {
        if (!_model._actorOnFloorRepository.Exists(targetCell)) return;

        if (_model._actorOnFloorRepository.GetActorType(targetCell) == (_model is CharacterModel ? 2 : 1))
        {
            BaseActorPresenter targetActor = _model._actorOnFloorRepository.GetActorObject(targetCell);

            if (targetActor != null)
            {
                if (_model is CharacterModel)
                {
                    targetActor.TakeDamage(damage);
                }
                else
                {
                    targetActor.TakeDamage(damage);
                }
            }
        }
    }

    public abstract void TakeDamage(int damage);
    public abstract void AddShield(int shield);
}