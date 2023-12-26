using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryHUD : ConstructUI {
    // Singleton
    public static InventoryHUD Instance;

    #region Properties

    [Space(10)]
    [SerializeField, Tooltip("How many inventoryslots the inventory should have")]
    private int InventorySlotAmount = 1;
    [SerializeField]
    private Texture2D testSprite;

    [Space(10)]
    [SerializeField, Header("Inventory Input")]
    private InputAction OpenInventory;
    [SerializeField]
    private InputAction MousePosition;
    [SerializeField]
    private InputAction MouseLeftPress;

    [Space(20)]
    [Header("Inventory Events")]
    [Space(5)]
    public UnityEvent OnInventoryClosed;
    public UnityEvent OnInventoryOpened;

    #endregion

    private VisualElement InventoryContainer; // Background panel of the inventory
    private InventoryItem ghostItem;

    private bool CanOpenInventory = false;
    private bool dragging;
    private Vector2 mousePos;
    private float screenHeight;


    private void Start() {
        // Init singleton
        Instance = this;
        screenHeight = Screen.height;
    }

    protected override void Construct() {
        // Apply 100% width and 100% height to root
        RootElement.AddToClassList("inv-root");
        RootElement.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        
        // Create inventory background
        InventoryContainer = CreateVisualElement<VisualElement>(RootElement, "inv-container");

        // Create inventoryslots
        CreateInventorySlots();

        // Create Ghostitem
        ghostItem = CreateVisualElement<InventoryItem>(RootElement, "item-ghostItem");
        // Hide ghostitem at start
        // HideElement(ghostItem);
        ghostItem.style.top = -1000;
        ghostItem.style.left = -1000;

        // DEBUG - Add some test items to slots
    }

    private void Update() {
        print(ghostItem.style.left + ", " + ghostItem.style.bottom);
        if (dragging) {
            // Move ghostItem with cursor
            ghostItem.style.left = (mousePos.x) - ghostItem.sizeX / 2f;
            ghostItem.style.top = (mousePos.y) - ghostItem.sizeY / 2f;
        }
    }

    private void OnOpenInventoryPressed(InputAction.CallbackContext context) {
        if (IsEnabled(RootElement)) {
            HideElement(RootElement);
            OnInventoryClosed?.Invoke();
        } else if (CanOpenInventory) {
            ShowElement(RootElement);
            OnInventoryOpened?.Invoke();
            // print(Draggable.sizeX + " ," + Draggable.sizeY);
        }
    }

    private void CreateInventorySlots() {
        for (int i = 1; i <= InventorySlotAmount; i++) {
            // Create slot
            InventorySlot newSlot = CreateVisualElement<InventorySlot>(InventoryContainer, "item-slot");
            if(i == 2) {
                AddItemToSlot(newSlot);
            }
        }
    }

    public void StartDrag(Background itemSprite) {
        // Enable ghostItem
        // ShowElement(ghostItem);
        // Set ghostItems image to the item being dragged
        ghostItem.style.backgroundImage = itemSprite;
        dragging = true;
    }

    private void StopDrag() {
        // Do stuff when drag ends
        // Hide ghostItem
        // HideElement(ghostItem);
        ghostItem.style.top = -1000;
        ghostItem.style.left = -1000;
        // Unset ghostItems Image
        ghostItem.style.backgroundImage = null;
        dragging = false;
    }

    #region GameStates

    public void OnWaitingToStart() {
        CanOpenInventory = false;
        HideElement(RootElement);
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

    #endregion

    #region Input

    private void OnEnable() {
        OpenInventory.Enable();
        OpenInventory.started += OnOpenInventoryPressed;
        MousePosition.Enable();
        // MousePosition.performed += OnMouseMove;
        MouseLeftPress.Enable();
        MouseLeftPress.canceled += OnMouseUp;
    }

    private void OnDisable() {
        OpenInventory.started -= OnOpenInventoryPressed;
        //MousePosition.performed -= OnMouseMove;
        MouseLeftPress.canceled -= OnMouseUp;
    }

    private void OnMouseMove(MouseMoveEvent evt) {
        mousePos = evt.mousePosition;
        // print("mouse moved, " + mousePos);
    }

    private void OnMouseUp(InputAction.CallbackContext context) {
        print("Mouse released");
        if (dragging) {
            StopDrag();
        }
    }

    private void OnMouseMove(InputAction.CallbackContext context) {
        mousePos = context.ReadValue<Vector2>();
    }

    #endregion

    #region Debug
    // Testing only -- Add some test items to item slots
    private void AddItemToSlot(InventorySlot slot) {
        ShowElement(slot.childItem);
        slot.childItem.style.backgroundImage = testSprite;
    }

    #endregion



}
