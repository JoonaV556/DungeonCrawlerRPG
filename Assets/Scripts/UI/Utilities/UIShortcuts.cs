using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Static utility class which contains useful shortcuts for UXML & USS management in C#
/// </summary>
public static class UIShortcuts
{
    /// <summary>
    /// Creates a visual element of specified type. Optionally adds it as a child to a specified parent & Assigns the specified classes to it
    /// </summary>
    /// <param name="parentElement">
    /// The parent element the new VisualElement will be added as a child to
    /// </param>
    /// <param name="@class">
    /// Optional USS class to assign to the new element
    /// </param>
    public static T CreateVisualElement<T>(VisualElement parentElement = null, string @class = null) where T : VisualElement, new() {
        // Create new element of specified type
        T newElement = new();

        // If parent is specified, add it as a child of specific parent element 
        if (parentElement != null) {
            parentElement.Add(newElement);
        }

        // Assign specified classes to the visual element
        if (@class != null) {
            newElement.AddToClassList(@class);
        }

        return newElement;
    }

    /// <summary>
    /// Hides a visual element by setting its displayStyle to none
    /// </summary>
    public static void HideElement(VisualElement element) {
        element.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Enables a visual element by setting its display to flex
    /// </summary>
    public static void ShowElement(VisualElement element) {
        element.style.display = DisplayStyle.Flex;
    }

    // Check if VisualElements display property is set to flex, i.e. if element is enabled
    public static bool IsEnabled(VisualElement element) {
        if (element.style.display == DisplayStyle.Flex) {
            return true;
        } else { return false; }
    }

    public static void EnableVisibility(VisualElement element) {
        element.style.visibility = Visibility.Visible;
    }

    public static void DisableVisibility(VisualElement element) {
        element.style.visibility = Visibility.Hidden;
    }
}
