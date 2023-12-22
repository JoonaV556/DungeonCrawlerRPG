using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class GameHUD : ConstructUI {

    [SerializeField]
    private string WaitingToStartText = "Press any key to start";
    [SerializeField]
    private string GameOverText = "Game over, press any key to try again";

    private ProgressBar healthBar;

    private VisualElement blackBG; // Black background panel

    private Label waitLabel;

    private Label overLabel;

    // Generate the ui
    protected override void Construct() {
        // Style root
        RootElement.AddToClassList("root");

        // Create healthbar
        healthBar = new ProgressBar();
        healthBar.title = "HP";
        healthBar.value = 100f;
        healthBar.AddToClassList("health-bar");
        // Add healthbar
        RootElement.Add(healthBar);

        // Add black background element
        blackBG = new VisualElement();
        blackBG.AddToClassList("panel-black");
        RootElement.Add(blackBG);

        // Add labels
        waitLabel = new Label();
        overLabel = new Label();
        waitLabel.text = WaitingToStartText;
        overLabel.text = GameOverText;
        blackBG.Add(waitLabel);
        blackBG.Add(overLabel);
        waitLabel.AddToClassList("text-centered");
        overLabel.AddToClassList("text-centered");
    }

    // Update the health UI element
    public void UpdateHealthBar(float newHealth) {
        healthBar.value = newHealth;
    }

    #region ReactToGameStates
    public void OnWaitingToStart() {     
        // Enable background
        ShowElement(blackBG);
        // Enable wait label
        ShowElement(waitLabel);
        // Hide healthbar
        HideElement(healthBar);
        // Hide game over label 
        HideElement(overLabel);
    }

    public void OnGameStarted() {
        // Hide wait UI
        // Enable health bar
        ShowElement(healthBar);
        HideElement(waitLabel);
        HideElement(blackBG);
    }

    public void OnGameOver() {
        // Enable game over ui
        // Disable health bar
        ShowElement(blackBG);
        ShowElement(overLabel);
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
