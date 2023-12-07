using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Handles behavior for entities with health, and which can be killed/destroyed
    #region Properties
    [SerializeField]
    private float MaxHealth = 10f;

    private float currentHealth;

    #endregion

    private void Start() {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(float DamageToTake) {
        if (currentHealth - DamageToTake <= 0f) {
            currentHealth = 0f;
            Die();
        } else {
            currentHealth -= DamageToTake;
        }
    }

    private void Die() {
        // Die logic 
        // Maybe trigger inspector assignable unity event?
        Destroy(this.gameObject);
    }



}
