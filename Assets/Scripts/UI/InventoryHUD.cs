using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryHUD : ConstructUI
{
    [Space(10)]
    [SerializeField, Tooltip("How many inventoryslots the inventory should have")]
    private int InventorySlotAmount = 1;

    [Space(10)]
    [SerializeField]
    private InputAction OpenInventoryAction;

    [Space(20)]
    [Header("Inventory Events")]
    [Space(5)]
    public UnityEvent OnInventoryClosed;
    public UnityEvent OnInventoryOpened;

    private VisualElement InventoryContainer; // Background panel of the inventory

    private bool CanOpenInventory = false;


    protected override void Construct() {
        // Apply 100% width and 100% height to root
        RootElement.AddToClassList("inv-root");
        // Create inventory background
        InventoryContainer = CreateVisualElement<VisualElement>(RootElement, "inv-container");
        // Create inventoryslots
        CreateInventorySlots();
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
        if (IsEnabled(InventoryContainer)) {
            HideElement(InventoryContainer);
            OnInventoryClosed?.Invoke();
        } else if (CanOpenInventory) {
            ShowElement(InventoryContainer);
            OnInventoryOpened?.Invoke();
            // print(Draggable.sizeX + " ," + Draggable.sizeY);
        }
    }

    #endregion

    private void CreateInventorySlots() {
        for (int i = 1; i <= InventorySlotAmount; i++) {
            // Create slot
            InventorySlot newSlot = CreateVisualElement<InventorySlot>(InventoryContainer, "inv-slot");
        }
    }

    public void OnWaitingToStart() {
        CanOpenInventory = false;
        HideElement(InventoryContainer);
    }

    public void OnGameStarted() {
        CanOpenInventory = true;
    }

    public void OnGameOver() {
        CanOpenInventory = false;
        HideElement(InventoryContainer);
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
