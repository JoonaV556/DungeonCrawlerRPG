using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// When pressed, closes the specified window by setting the visualelement display to none
/// </summary>
public class CloseWindowButton : Button
{
    /// <summary>
    /// The visual element to hide when the button is clicked. The button won't work if this is not assigned
    /// </summary>
    public VisualElement ElementToClose;

    public new class UxmlFactory : UxmlFactory<CloseWindowButton> { }

    public CloseWindowButton() {
        this.AddToClassList("closeBtn");
        // Sub to clicked event
        this.clicked += OnClicked; 
        // Hide text label
        this.text = string.Empty;
    }

    private void OnClicked() {
        UIShortcuts.HideElement(ElementToClose);
    }
}
