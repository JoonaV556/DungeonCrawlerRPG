using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class GameHUD : ConstructUI {

    private ProgressBar healthBar;

    // Generate the ui
    protected override void Construct() {
        // Style root
        RootElement.AddToClassList("root");

        // Create healthbar
        healthBar = UIShortcuts.CreateVisualElement<ProgressBar>(RootElement, "health-bar");
        healthBar.title = "HP";
        healthBar.value = 100f;
    }

    // Update the health UI element
    public void UpdateHealthBar(float newHealth) {
        healthBar.value = newHealth;
    }

    #region ReactToGameStates
    public void OnWaitingToStart() {
        // Hide healthbar
        HideElement(healthBar);
    }

    public void OnGameStarted() {
        // Hide wait UI
        // Enable health bar
        ShowElement(healthBar);
    }

    public void OnGameOver() {
    }

    // Choose how to react
    public void OnGameStateChanged(GameState newState) {
        switch (newState) {
            case GameState.WaitingToStart:
                // Pause game at start
                OnWaitingToStart();
                break;

            case GameState.Playing:
                // Exit pause
                OnGameStarted();
                break;

            case GameState.GameOver:
                // Pause game when game ends
                OnGameOver();
                break;
        }
    }
    #endregion

}
