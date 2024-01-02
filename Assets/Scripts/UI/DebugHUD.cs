using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DebugHUD : ConstructUI {
    [Space(10)]
    [SerializeField, Tooltip("Customizable input action which can be used to enable the DebugHUD")]
    private InputAction DebugAction;
    [Space(10)]

    [Header("Debug values")]
    [Space(5)]
    [SerializeField]
    private float DamageAmount = 10f;
    [SerializeField]
    private float HealAmount = 10f;

    [Header("Button press events")]
    [Space(5)]
    public UnityEvent<float> OnDamagePlayerPressed;
    public UnityEvent<float> OnHealPlayerPressed;

    private VisualElement debugPanel;
    private Label debugLabel;
    private Button damagePlayerBtn;
    private Button healPlayerBtn;

    private bool canEnableHUD = false;

    protected override void Construct() {
        // Create elements
        debugPanel = UIShortcuts.CreateVisualElement<VisualElement>(RootElement, "debug-menu--background");
        debugLabel = UIShortcuts.CreateVisualElement<Label>(debugPanel, "debug-menu--label");
        damagePlayerBtn = UIShortcuts.CreateVisualElement<Button>(debugPanel);
        healPlayerBtn = UIShortcuts.CreateVisualElement<Button>(debugPanel);
        // Init labels
        debugLabel.text = "Debug Menu";
        damagePlayerBtn.text = "DamagePlayer";
        healPlayerBtn.text = "HealPlayer";
        // Sub to button events
        damagePlayerBtn.clicked += onDamagePlayerPressed;
        healPlayerBtn.clicked += onHealPlayerPressed;
    }

    // Toggle the debug hud when the associated input action is triggered
    private void ToggleDebugHUD(InputAction.CallbackContext context) {
        if (UIShortcuts.IsEnabled(debugPanel)) {
            UIShortcuts.HideElement(debugPanel);
        } else if (canEnableHUD){
            UIShortcuts.ShowElement(debugPanel);
        }
    }

    // Enable the input action and sub to the input action events
    private void OnEnable() {
        DebugAction.Enable();
        DebugAction.started += ToggleDebugHUD;
    }
    // Unsub to the input action events
    private void OnDisable() {
        DebugAction.started -= ToggleDebugHUD;
        damagePlayerBtn.clicked -= onDamagePlayerPressed;
        healPlayerBtn.clicked -= onHealPlayerPressed;
    }

    #region ReactToButtonPresses

    private void onDamagePlayerPressed() {
        OnDamagePlayerPressed?.Invoke(DamageAmount);
    }

    private void onHealPlayerPressed() {
        OnHealPlayerPressed?.Invoke(HealAmount);
    }

    #endregion

    #region ReactToGameStates

    public void OnWaitingToStart() {
        UIShortcuts.HideElement(debugPanel);
        canEnableHUD = false;
    }

    public void OnGameStarted() {
        canEnableHUD = true;
    }

    public void OnGameOver() {
        UIShortcuts.HideElement(debugPanel);
        canEnableHUD = false;
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
