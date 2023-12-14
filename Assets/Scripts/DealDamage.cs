using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour {
    // Attach to 2d colliders which should deal damage when something hits them (weapon attack colliders etc.)
    
    /// <summary>
    /// How much damage is dealt to the health component of the other object when hit (if the object has one)
    /// </summary>
    [SerializeField]
    private float DamageToDeal = 100f;
    [SerializeField]
    private Transform DealerTransform;

    void OnCollisionEnter2D(Collision2D collision) {
        // Deal damage to the other object if it has a Health component

        // Debug
        // print("Hit other object");
        // HitObjects.Add(collision.gameObject);

        Health otherObjHealth = collision.gameObject.GetComponent<Health>();
        if (otherObjHealth != null) {
            otherObjHealth.TakeDamage(DamageToDeal, DealerTransform);
            // Debug
            // print("Dealt damage");
        }
    }
}
