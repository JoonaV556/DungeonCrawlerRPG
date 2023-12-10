using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// How many seconds needs to pass before target is removed from the perceived targets list, if the target has exited out of sight
    /// </summary>
    [SerializeField]
    private float TargetLossTimer = 5f;

    private float currentTime = 0f;
    private float elapsedTime = 0f;
    private List<Perceivable> perceivablesInProximity = new();
    private List<LostSightPerceivable> LostSightPerceivables = new();

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
        if (perceivable == null) { return; }
        // Remove perceivable from visual checks
        if (perceivablesInProximity.Contains(perceivable)) {
            perceivablesInProximity.Remove(perceivable);
        }
        // Start removal cooldown
        if (PerceivedTargets.Contains(perceivable)) {
            // Check if target is already waiting to be removed
            bool IsTargetAlreadyUpForRemoval = IsTargetWaitingForRemoval(perceivable);
            // If target is not already to be removed, prepare it for removal
            if (!IsTargetAlreadyUpForRemoval) {
                LostSightPerceivable percToAdd = new LostSightPerceivable();
                percToAdd.Target = perceivable;
                // Set the time in future when the target should be forgotten, if visual sight is not regained
                percToAdd.TimeOfRemoval = Time.time + TargetLossTimer;
                // Add the perceivable to be tracked
                LostSightPerceivables.Add(percToAdd);
            }
        }
    }

    private void Update() {
        // Do visibility checks at specific interval
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
            Vector3 TargetVector = perc.VisualTransform.position - VisibilityCheckOrigin.position;
            // Do raycast
            RaycastHit2D HitResult = Physics2D.Raycast(RaycastOrigin, TargetVector, Mathf.Infinity, VisibilityLayerMask);
            Debug.DrawRay(RaycastOrigin, TargetVector, Color.green, 0.3f, false);
            bool CanSeeTarget = HitResult.transform == percTransform;
            bool IsTargetAlreadyPerceived = PerceivedTargets.Contains(perc);
            // Prepare new targets if they can be seen
            if (CanSeeTarget && !IsTargetAlreadyPerceived) {
                PerceivablesToAdd.Add(perc);
            }
            // Prepare old targets for removal if they cannot be seen
            if (IsTargetAlreadyPerceived && !CanSeeTarget) {
                // Check if target is already waiting to be removed
                bool IsTargetAlreadyUpForRemoval = IsTargetWaitingForRemoval(perc);
                // If target is not already to be removed, prepare it for removal
                if (!IsTargetAlreadyUpForRemoval) {
                    LostSightPerceivable percToAdd = new LostSightPerceivable();
                    percToAdd.Target = perc;
                    // Set the time in future when the target should be forgotten, if visual sight is not regained
                    percToAdd.TimeOfRemoval = Time.time + TargetLossTimer;
                    // Add the perceivable to be tracked
                    LostSightPerceivables.Add(percToAdd);
                }
            }
        }
        // Set perceived targets to be removed if they have been out of sight for long enough
        List<LostSightPerceivable> lsPercsToRemove = new();
        if (LostSightPerceivables.Count >= 0) {
            foreach (LostSightPerceivable lsPerc in LostSightPerceivables) {
                if (Time.time >= lsPerc.TimeOfRemoval) {
                    lsPercsToRemove.Add(lsPerc);
                    PerceivablesToRemove.Add(lsPerc.Target);                   
                }
            }
        }


        // Remove tracked lsPercs on cooldown
        LostSightPerceivables = LostSightPerceivables.Except(lsPercsToRemove).ToList();
        // Remove old targets
        PerceivedTargets = PerceivedTargets.Except(PerceivablesToRemove).ToList();
        // Add new targets to perceived
        PerceivedTargets = PerceivedTargets.Union(PerceivablesToAdd).ToList();

        // Debug
        currentTime = Time.time;
    }

    private bool IsTargetWaitingForRemoval(Perceivable perc) {
        // Check if the target is already up for removal
        foreach (LostSightPerceivable lsPerc in LostSightPerceivables) {
            if (lsPerc.Target == perc) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// A Perceivable target of which the perceiver has lost sight of. Will be removed from PerceivedTargets if sight is not regained before removal time
    /// </summary>
    protected class LostSightPerceivable {
        public Perceivable Target; // The target which is tracked
        public float TimeOfRemoval; // The point of time in future, at which the target will be removed if visual sight is not regained
    }
}
