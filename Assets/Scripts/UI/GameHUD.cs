using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class GameHUD : MonoBehaviour {
    [SerializeField]
    private UIDocument uiDoc;

    [SerializeField]
    private StyleSheet styles;

    [SerializeField]
    private string WaitingToStartText = "Press any key to start";
    [SerializeField]
    private string GameOverText = "Game over, press any key to try again";

    private VisualElement root;

    private ProgressBar healthBar;

    private VisualElement blackBG; // Black background panel

    private Label waitLabel;

    private Label overLabel;

    private VisualElement DebugMenuRoot;



    // Generate UI at game start
    private void Awake() {
        Generate();
    }

    // Refresh the UI in editor after changes have been saved
    private void OnValidate() {
        if (Application.isPlaying) return;
        Generate();
    }

    // Generate the ui
    private void Generate() {
        // Create new visual tree asset 
        uiDoc.visualTreeAsset = ScriptableObject.CreateInstance<VisualTreeAsset>();

        // Get root
        root = uiDoc.rootVisualElement;
        // Add our custom stylesheet to the document
        root.styleSheets.Add(styles);
        // Style root
        root.AddToClassList("root");

        // Create healthbar
        healthBar = new ProgressBar();
        healthBar.title = "HP";
        healthBar.value = 100f;
        healthBar.AddToClassList("health-bar");
        // Add healthbar
        root.Add(healthBar);

        // Add black background element
        blackBG = new VisualElement();
        blackBG.AddToClassList("panel-black");
        root.Add(blackBG);

        // Add labels
        waitLabel = new Label();
        overLabel = new Label();
        waitLabel.text = WaitingToStartText;
        overLabel.text = GameOverText;
        blackBG.Add(waitLabel);
        blackBG.Add(overLabel);
        waitLabel.AddToClassList("text-centered");
        overLabel.AddToClassList("text-centered");

        // Add debug menu
        DebugMenuRoot = CreateVisualElement<VisualElement>(root, new[] { "debug-menu--background"});
    }

    /// <summary>
    /// Construct a visual element of type. Optionally add it as a child to a specified parent & Assign specified classes to it
    /// </summary>
    private VisualElement CreateVisualElement<T>(VisualElement parentElement = null, string[] classes = null) where T : VisualElement, new() {
        // Create new element of specified type
        T newElement = new T();

        // If parent is specified, add it as a child of specific parent element 
        if (parentElement != null) {
            parentElement.Add(newElement);
        }

        // Assign specified classes to the visual element
        if (classes != null) {
            foreach (string className in classes) {
                newElement.AddToClassList(className);
            }        
        }

        return newElement;
    }

    // Update the health UI element
    public void UpdateHealthBar(float newHealth) {
        healthBar.value = newHealth;
    }

    // Disables a visual element by setting its display to none
    private void HideElement(VisualElement element) {
        element.style.display = DisplayStyle.None;
    }

    // Enables a visual element by setting its display to flex
    private void ShowElement(VisualElement element) {
        element.style.display = DisplayStyle.Flex;
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
