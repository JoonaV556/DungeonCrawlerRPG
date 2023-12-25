using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryHUD : ConstructUI
{
    [Space(10)]
    [SerializeField]
    private InputAction OpenInventoryAction;

    [Space(20)]
    [Header("Inventory Events")]
    [Space(5)]
    public UnityEvent OnInventoryClosed;
    public UnityEvent OnInventoryOpened;

    private VisualElement InventoryPanel; // Background panel of the inventory
    private VisualElement Draggable;
    private VisualElement Draggable2;

    private bool CanOpenInventory = false;


    protected override void Construct() {
        // Apply 100% width and 100% height to root
        RootElement.AddToClassList("root");
        // Create inventory background
        InventoryPanel = CreateVisualElement<VisualElement>(RootElement, "inventory-panel");
        Draggable = CreateVisualElement<VisualElement>(InventoryPanel, "draggable");
        Draggable.AddManipulator(new ExampleDragger());
        Draggable2 = CreateVisualElement<VisualElement>(InventoryPanel, "draggable");
        Draggable2.AddManipulator(new ExampleDragger());
        Draggable2.style.left = 500;
        Draggable2.style.top = 500;
        Draggable2.style.backgroundColor = Color.red;

    }

    private void Update() {
        // print(Mouse.current.position.ReadValue());
    }

    #region OpenInventoryStuff

    private void OnEnable() {
        OpenInventoryAction.Enable();
        OpenInventoryAction.started += OnOpenInventoryPressed;
    }

    private void OnDisable() {
        OpenInventoryAction.started -= OnOpenInventoryPressed;
    }

    private void OnOpenInventoryPressed(InputAction.CallbackContext context) {
        if (IsEnabled(InventoryPanel)) {
            HideElement(InventoryPanel);
            OnInventoryClosed?.Invoke();
        } else if (CanOpenInventory) {
            ShowElement(InventoryPanel);
            OnInventoryOpened?.Invoke();
            // print(Draggable.sizeX + " ," + Draggable.sizeY);
        }
    }

    #endregion

    public void OnWaitingToStart() {
        CanOpenInventory = false;
        HideElement(InventoryPanel);
    }

    public void OnGameStarted() {
        CanOpenInventory = true;
    }

    public void OnGameOver() {
        CanOpenInventory = false;
        HideElement(InventoryPanel);
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
}
