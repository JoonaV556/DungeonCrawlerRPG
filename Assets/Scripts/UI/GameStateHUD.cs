using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameStateHUD : ConstructUI {
    [SerializeField]
    private string WaitingToStartText = "Press any key to start";
    [SerializeField]
    private string GameOverText = "Game over, press any key to try again";

    private VisualElement blackBG; // Black background panel
    private Label waitLabel;
    private Label overLabel;

    protected override void Construct() {
        // Add black background element
        blackBG = CreateVisualElement<VisualElement>(RootElement, "panel-black");

        // Add labels
        waitLabel = CreateVisualElement<Label>(blackBG, "text-centered");
        overLabel = CreateVisualElement<Label>(blackBG, "text-centered");
        waitLabel.text = WaitingToStartText;
        overLabel.text = GameOverText;
    }

    #region ReactToGameStates

    public void OnWaitingToStart() {
        // Enable background
        ShowElement(blackBG);
        // Enable wait label
        ShowElement(waitLabel);
        // Hide healthbar
        // Hide game over label 
        HideElement(overLabel);
    }

    public void OnGameStarted() {
        // Hide wait UI
        // Enable health bar
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
