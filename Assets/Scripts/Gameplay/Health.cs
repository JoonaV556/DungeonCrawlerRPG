using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    // Handles behavior for entities with health, and which can be killed/destroyed
    #region Properties

    /// <summary>
    /// How much health the object should have at start
    /// </summary>
    [SerializeField]
    protected float MaxHealth = 10f;

    /// <summary>
    /// How much health the object has currently
    /// </summary>
    protected float currentHealth;

    #endregion

    private void Start() {
        currentHealth = MaxHealth;
    }

    /// <summary>
    /// Used to deal damage to this health component
    /// </summary>
    public void TakeDamage(float DamageToTake) {
        if (currentHealth - DamageToTake <= 0f) {
            currentHealth = 0f;
            Die();
        } else {
            currentHealth -= DamageToTake;
        }
    }

    /// <summary>
    /// Triggered when health reaches zero
    /// </summary>
    protected abstract void Die();
}
