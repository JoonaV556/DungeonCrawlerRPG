using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualProximityPerceiver2D : Perceiver {
    // Perceives perceivables using a trigger collider and raycast
    // Collider is used to get list of considered targets
    // Raycast is done to check if targets can be seen


    /// <summary>
    /// Frequency of the visibility checks in seconds
    /// </summary>
    [SerializeField]
    private float VisibilityCheckFrequency = 0.2f;
    [SerializeField]
    private LayerMask VisibilityLayerMask;
    [SerializeField]
    private Transform VisibilityCheckOrigin;

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
        if (perceivable != null) {
            if (perceivablesInProximity.Contains(perceivable)) {
                perceivablesInProximity.Remove(perceivable);
            }
            if (PerceivedTargets.Contains(perceivable)) {
                PerceivedTargets.Remove(perceivable);
            }
        }
    }

    private void Update() {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= VisibilityCheckFrequency) {
            elapsedTime = 0f;
            DoVisibilityCheck();
        }
    }

    /// <summary>
    /// Checks if targets in proximity can be seen. If yes, the targets are added to the PerceivedTargets list
    /// </summary>
    private void DoVisibilityCheck() {
        Vector3 RaycastOrigin = VisibilityCheckOrigin.position; // Cache origin position of raycast before loop
        List<Perceivable> PerceivablesToRemove = new();
        List<Perceivable> PerceivablesToAdd = new();
        foreach (Perceivable perc in perceivablesInProximity) {
            Transform percTransform = perc.transform;
            // Calculate vector facing towards target
            Vector3 TargetVector = percTransform.position - VisibilityCheckOrigin.position;
            // Do raycast
            RaycastHit2D HitResult = Physics2D.Raycast(RaycastOrigin, TargetVector, Mathf.Infinity, VisibilityLayerMask);
            Debug.DrawRay(RaycastOrigin, TargetVector, Color.green, 0.3f, false);

            print(HitResult.collider.gameObject.name);

            bool CanSeeTarget = HitResult.transform == percTransform;
            bool IsTargetAlreadyPerceived = PerceivedTargets.Contains(perc);
            // Prepare new targets if they can be seen
            if (CanSeeTarget && !IsTargetAlreadyPerceived) {
                PerceivablesToAdd.Add(perc);
            }
            // Prepare old targets for removal if they cannot be seen
            if (IsTargetAlreadyPerceived && !CanSeeTarget) {
                PerceivablesToRemove.Add(perc);
            }
        }
        // Add new targets
        foreach (Perceivable perc in PerceivablesToAdd) {
            PerceivedTargets.Add(perc);
        }
        // Remove old targets
        foreach (Perceivable perc in PerceivablesToRemove) {
            PerceivedTargets.Remove(perc);
        }
    }
}
