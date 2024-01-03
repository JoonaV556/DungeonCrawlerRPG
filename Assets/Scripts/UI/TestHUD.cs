using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestHUD : ConstructUI {

    [SerializeField]
    private StyleSheet closeBtnStyle;
    [SerializeField]
    private StyleSheet windowStyle;
    [SerializeField]
    private StyleSheet slotStyle;

    private CloseWindowButton closeButton;
    private Window window;

    protected override void Construct() {
        RootElement.AddToClassList("root");

        // test creating a window
        TestWindow();
    }

    private void TestWindow() {
        // Create parent window
        window = UIShortcuts.CreateVisualElement<Window>(RootElement);
        window.styleSheets.Add(windowStyle);

        // Test adding content to the window
        RootElement.styleSheets.Add(slotStyle);
        window.AddContent(UIShortcuts.CreateVisualElement<InventorySlot>(null, "item-slot"));
        window.AddContent(UIShortcuts.CreateVisualElement<InventorySlot>(null, "item-slot"));
        window.AddContent(UIShortcuts.CreateVisualElement<InventorySlot>(null, "item-slot"));
        window.AddContent(UIShortcuts.CreateVisualElement<InventorySlot>(null, "item-slot"));

        // Create the close button
        closeButton = UIShortcuts.CreateVisualElement<CloseWindowButton>(window.header);
        // Assign the window to close
        closeButton.ElementToClose = window;
        // Style the button
        closeButton.styleSheets.Add(closeBtnStyle);
        closeButton.text = "X";
    }
}
