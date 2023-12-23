using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DraggableElement : VisualElement
{
    public DraggableElement () {
        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<MouseUpEvent>(OnMouseUp);
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
    }

    private void OnMouseDown(MouseDownEvent evt) {
        Debug.Log("epic");
    }

    private void OnMouseUp(MouseUpEvent evt) {

    }

    private void OnMouseMove(MouseMoveEvent evt) {
        Debug.Log(evt.mousePosition);
    }
}
