using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent_Vector2 OnReceiveDamage;
    public UnityEvent<float> OnTakeDamage;

    private void Start() {
        currentHealth = MaxHealth;
    }

    /// <summary>
    /// Used to deal damage to this health component
    /// </summary>
    public void TakeDamage(float DamageToTake, Transform DamageGiver) {
        print(gameObject.name + " Took damage");
        Vector2 DamageGiverPost = DamageGiver.transform.position;
        OnReceiveDamage?.Invoke(DamageGiver.transform.position);
        if (currentHealth - DamageToTake <= 0f) {
            currentHealth = 0f;
            OnTakeDamage?.Invoke(currentHealth);
            Die();
        } else {
            currentHealth -= DamageToTake;
            OnTakeDamage?.Invoke(currentHealth);
        }
    }

    /// <summary>
    /// Triggered when health reaches zero
    /// </summary>
    protected abstract void Die();
}
