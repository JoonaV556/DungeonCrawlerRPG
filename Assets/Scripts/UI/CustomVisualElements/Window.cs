using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Window : VisualElement
{
    private VisualElement content;
    public VisualElement header;

    public Window() {
        // Style the window
        AddToClassList("window");
        // Create child elements
        header = UIShortcuts.CreateVisualElement<VisualElement>(this, "window-header");
        content = UIShortcuts.CreateVisualElement<VisualElement>(this, "window-content");
    }

    public new class UxmlFactory : UxmlFactory<Window> { }

    public void AddContent(VisualElement elementToAdd) {
        content.Add(elementToAdd);
    }
}
