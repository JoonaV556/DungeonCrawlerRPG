using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualProximityPerceiver2D : Perceiver
{
    // Perceives perceivables using a trigger collider and raycast
    // Collider is used to get list of considered targets
    // Raycast is done to check if targets can be seen


    /// <summary>
    /// Frequency of the visibility checks in seconds
    /// </summary>
    [SerializeField]
    private float NextVisibilityCheckTime = 0.2f;

    float elapsedTime = 0f;
    private List<Perceivable> perceivablesInProximity = new List<Perceivable>();

    void OnTriggerEnter2D(Collider2D otherCollider) {
        // Add perceivables which have entered proximity
        Perceivable perceivable = otherCollider.gameObject.GetComponent<Perceivable>();
        if (perceivable != null) {
            perceivablesInProximity.Add(perceivable);
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider) {
        // Remove perceivables which are no longer in proximity
        Perceivable perceivable = otherCollider.gameObject.GetComponent<Perceivable>();
        if (perceivable != null && perceivablesInProximity.Contains(perceivable)) {
            perceivablesInProximity.Remove(perceivable);
        }
    }

    private void Update() {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= NextVisibilityCheckTime) {           
            NextVisibilityCheckTime += elapsedTime;
            DoVisibilityCheck();
        }
    }

    private void DoVisibilityCheck() {
        // TODO
        // Check if each perceivable can be seen by raycasting
        // if yes, perceivable to perceivedtargets List
    }
}
