using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{

    private void Start() {
        // Subscribe to callbacks
        ObstacleHealth.OnObstacleDestroyed += UpdateNavGridNearObstacle;  
    }

    /// <summary>
    /// Update the navigation grid (faster)
    /// </summary>
    public static void UpdateNavGrid() {

    }

    /// <summary>
    /// Recalculate the whole navigation grid (takes more time)
    /// </summary>
    public static void RescanNavGrid() { 

    }

    /// <summary>
    /// Update the navigation grid near a specific collider (Use for position targeted updates - really fast)
    /// </summary>
    public static void UpdateNavGridNearObstacle(Collider2D Collider) {
        AstarPath.active.UpdateGraphs(Collider.bounds);
    }
}
