using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;

/// <summary>
/// Generic runtime window which can be dragged around the screen
/// </summary>
public class DraggableWindow : VisualElement
{
    // Stylesheet: Window

    protected VisualElement content;
    protected CloseWindowButton closeWindowButton;


    public VisualElement Header {  get; protected set; }

    protected DragManipulator dragManipulator;

    public DraggableWindow() {
        // Style the window
        AddToClassList("window");

        // Create child elements
        Header = UIShortcuts.CreateVisualElement<VisualElement>(this, "window-header");
        content = UIShortcuts.CreateVisualElement<VisualElement>(this, "window-content");

        // Create close button
        closeWindowButton = UIShortcuts.CreateVisualElement<CloseWindowButton>(Header);
        closeWindowButton.CloseTarget = this;

        // Register events
        Header.RegisterCallback<MouseEnterEvent>(Header_OnMouseEnter);
        Header.RegisterCallback<MouseLeaveEvent>(Header_OnMouseLeave);

        // Add drag manipulator
        dragManipulator = new DragManipulator();
        dragManipulator.BringToFrontWhileDragging = true;
        this.AddManipulator(dragManipulator);
        // Print debug info when dropped
        RegisterCallback<DropEvent>(evt => Debug.Log($"{evt.target} dropped on {evt.droppable}"));
        // Prevent dragging by default
        dragManipulator.enabled = false;
    }

    public new class UxmlFactory : UxmlFactory<DraggableWindow> { }

    /// <summary>
    /// Adds visual elements to the content container
    /// </summary>
    /// <param name="elementToAdd">Element to add inside the content container</param>
    public void AddContent(VisualElement elementToAdd) {
        content.Add(elementToAdd);
    }

    private void Header_OnMouseEnter(MouseEnterEvent evt) {
        // Allow dragging when mouse is on header
        dragManipulator.enabled = true;
    }

    private void Header_OnMouseLeave(MouseLeaveEvent evt) {
        dragManipulator.enabled = false;
    }
}
