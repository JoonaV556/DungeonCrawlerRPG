using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot : VisualElement
{

    public InventorySlot() {
        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
    }

    private void OnMouseDown(MouseDownEvent evt) {

    }

    private void OnMouseMove(MouseMoveEvent evt) {

    }
}
