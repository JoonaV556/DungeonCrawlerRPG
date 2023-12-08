using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualProximityPerceiver2D : Perceiver
{
    // Perceives perceivables using a trigger collider and raycast
    // Collider is used to get list of considered targets
    // Raycast is done to check if targets can be seen

    private List<Perceivable> perceivablesInProximity = new List<Perceivable>();

    void OnCollisionEnter2D(Collision2D collision) {
        // Add perceivables which have entered proximity
        Perceivable perceivable = collision.gameObject.GetComponent<Perceivable>();
        if (perceivable != null) {
            perceivablesInProximity.Add(perceivable);
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        // Remove perceivables which are no longer in proximity
        Perceivable perceivable = collision.gameObject.GetComponent<Perceivable>();
        if (perceivablesInProximity.Contains(perceivable)) {
            perceivablesInProximity.Remove(perceivable);
        }
    }
}
