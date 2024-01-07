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
    private InventoryItemUI ghostItem;
    private InventorySlotUI dropSlot = null; // Slot into which dragged item is dropped, if one exists
    private InventorySlotUI oldSlot = null; // Slot form which the item is being dragged from

    private bool CanOpenInventory = false;
    private bool dragging;
    private Vector2 mouseEventPos;
    private Vector2 mouseScreenPos;
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
        InventoryContainer = UIShortcuts.CreateVisualElement<VisualElement>(RootElement, "inv-container");

        // Create inventoryslots
        CreateInventorySlots();

        // Create Ghostitem
        ghostItem = UIShortcuts.CreateVisualElement<InventoryItemUI>(RootElement, "item-ghostItem");
        // Hide ghostitem at start
        UIShortcuts.DisableVisibility(ghostItem);
        // Make ghostItem not react to mouse events (CRITICAL for slot detection)
        ghostItem.pickingMode = PickingMode.Ignore;
        // Set ghost items position to hide it from screen
        // ghostItem.style.top = -1000;
        // ghostItem.style.left = -1000;
    }

    private void Update() {
        // print(mouseScreenPos);
        if (dragging) {
            // Move ghostItem with mouse event position
            // ghostItem.style.left = (mouseEventPos.x) - ghostItem.sizeX / 2f;
            // ghostItem.style.top = (mouseEventPos.y) - ghostItem.sizeY / 2f;
            // Move with mouse screen position
            ghostItem.style.left = (mouseScreenPos.x) - ghostItem.sizeX / 2f;
            ghostItem.style.bottom = (mouseScreenPos.y) - ghostItem.sizeY / 2f;
        }
    }

    private void OnOpenInventoryPressed(InputAction.CallbackContext context) {
        if (UIShortcuts.IsEnabled(RootElement)) {
            UIShortcuts.HideElement(RootElement);
            OnInventoryClosed?.Invoke();
        } else if (CanOpenInventory) {
            UIShortcuts.ShowElement(RootElement);
            OnInventoryOpened?.Invoke();
            // print(Draggable.sizeX + " ," + Draggable.sizeY);
        }
    }

    private void CreateInventorySlots() {
        for (int i = 1; i <= InventorySlotAmount; i++) {
            // Create slot
            InventorySlotUI newSlot = UIShortcuts.CreateVisualElement<InventorySlotUI>(InventoryContainer, "item-slot");
            if (i == 2) {
                AddItemToSlot(newSlot);
            }
        }
    }

    public void StartDrag(InventorySlotUI startSlot, Background itemSprite) {
        // Store the slot from which the item is being dragged from
        oldSlot = startSlot;
        // Enable ghostItem
        UIShortcuts.EnableVisibility(ghostItem);
        // Set ghostItems image to the item being dragged
        ghostItem.style.backgroundImage = itemSprite;
        dragging = true;
    }

    private void EndDrag() {
        // Check if item can be dropped in a slot
        if (CanDropInSlot(out InventorySlotUI slot)) {
            // Remove the item from the old slot
            oldSlot.ClearItem();
            // Drop item into the new slot
            DropItemInSlot(slot);
        }

        // Do stuff when drag ends -->

        // Unset the old slot
        oldSlot = null;
        // Unset dropSlot
        dropSlot = null;

        // Hide ghostItem
        UIShortcuts.DisableVisibility(ghostItem);
        // Unset ghostItems Image
        ghostItem.style.backgroundImage = null;

        // Stop dragging
        dragging = false;
    }

    // Checks if there's a slot into which the item can be dropped into
    private bool CanDropInSlot(out InventorySlotUI slot) {
        // Return false if no slot exists
        if (dropSlot == null) {
            slot = null;
            return false;
        }
        // If dropSlot exists, check if the slot already has an item
        if (dropSlot.HasItem(out Background itemImage)) {
            slot = null;
            return false;
        }
        // Drop slot exists and item can be dropped into it
        // Return the slot to drop into
        slot = dropSlot;
        // Return true
        return true;
    }

    // Drops the dragged item into a slot
    private void DropItemInSlot(InventorySlotUI slot) {
        // Testing drop behavior: 
        slot.SetItem(testSprite);
    }

    // Triggered when cursor moves over an inventory slot
    public void OnCursorEnterSlot(InventorySlotUI slot) {
        if (dragging) {
            dropSlot = slot;
            // print("Got dropSlot");
        }
    }

    // Triggered when cursor moves out of an inventory slot
    public void OnCursorLeaveSlot(InventorySlotUI slot) {
        if (slot == dropSlot) {
            dropSlot = null;
            //print("Lost dropSlot");
        }
    }

    #region GameStates

    public void OnWaitingToStart() {
        CanOpenInventory = false;
        UIShortcuts.HideElement(RootElement);
    }

    public void OnGameStarted() {
        CanOpenInventory = true;
    }

    public void OnGameOver() {
        CanOpenInventory = false;
        UIShortcuts.HideElement(InventoryContainer);
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
        MousePosition.performed += OnInputMouseMove;
        MouseLeftPress.Enable();
        MouseLeftPress.canceled += OnMouseUp;
    }

    private void OnDisable() {
        OpenInventory.started -= OnOpenInventoryPressed;
        MousePosition.performed -= OnInputMouseMove;
        MouseLeftPress.canceled -= OnMouseUp;
    }

    private void OnMouseMove(MouseMoveEvent evt) {
        mouseEventPos = evt.mousePosition;
        // print("mouse moved, " + mousePos);
    }

    private void OnMouseUp(InputAction.CallbackContext context) {
        // print("Mouse released");
        if (dragging) {
            EndDrag();
        }
    }

    private void OnInputMouseMove(InputAction.CallbackContext context) {
        mouseScreenPos = context.ReadValue<Vector2>();
    }

    #endregion

    #region Debug
    // Testing only -- Add some test items to item slots
    private void AddItemToSlot(InventorySlotUI slot) {
        UIShortcuts.ShowElement(slot.childItem);
        slot.childItem.style.backgroundImage = testSprite;
    }

    #endregion

}
