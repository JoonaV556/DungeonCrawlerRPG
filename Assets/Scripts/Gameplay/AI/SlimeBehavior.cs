using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehavior : MonoBehaviour
{
    [SerializeField]
    AIPath AIPath;
    [SerializeField]
    Perceiver SlimePerceiver;

    private bool SeesPlayer = false;
    private Transform PlayerTransform;

    // TODO
    // Check if has player target
    // If yes - Set destination to player target

    private void Update() {
        if (SeesPlayer) {
            AIPath.destination = PlayerTransform.position;
        }
    }
}
