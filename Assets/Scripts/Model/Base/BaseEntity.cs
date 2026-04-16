public abstract class BaseEntity : IDamageable {
    public int maxHealth { get; private set; }
    public int currentHealth { get; private set; }

    public virtual void SetUpEntity(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount) {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // Die();
        }
    }

    // public abstract void ApplyEffect(Effect effect);
    
    // protected virtual void Die() {
    // }
}