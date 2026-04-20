using UnityEngine;

public abstract class BaseActorModel 
{   
    public const float Y_WORD_POSITION = 1.15f;

    public ScriptableObject actorData { get; private set; }

    public ActorOnFloorRepository _actorOnFloorRepository { get; private set; }
    public GridFloorRepository _gridFloorRepository { get; private set; }

    public int maxHealth { get; private set; }
    public int currentHealth { get; set; }
    public int currentShield  { get; private set; }

    public int currentActorCell { get; set; }

    public int baseAttackDamage { get; private set; }

    public virtual void SetUpActor(int maxHealth, ScriptableObject actorData,  int baseAttackDamage = 10)
    {
        this.actorData = actorData;

        _actorOnFloorRepository = ActorOnFloorRepository.Instance;
        _gridFloorRepository = GridFloorRepository.Instance;

        this.maxHealth = maxHealth;
        this.baseAttackDamage = baseAttackDamage;
        currentHealth = maxHealth;
    }

    public virtual bool TakeDamage(int amount) 
    {
        if (currentShield > 0)
        {
            int damageToShield = Mathf.Min(amount, currentShield);
            currentShield -= damageToShield;
            amount -= damageToShield;
        }

        if (amount > 0)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                    return false;
            }
        }

        return true;
    }

    public virtual void AddShield(int amount) 
    {
        currentShield += amount;
    }

    public Vector2? GetTargetPosition(int targetCellIndex)
    {

        if (!_gridFloorRepository.Exists(targetCellIndex))
        {
            Debug.LogWarning($"Cell index {targetCellIndex} does not exist in Repository.");
            return null;
        }

        return new Vector2(_gridFloorRepository.Get(targetCellIndex).transform.localPosition.x, Y_WORD_POSITION);
    }
}