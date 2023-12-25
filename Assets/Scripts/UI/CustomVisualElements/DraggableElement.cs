using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DraggableElement : VisualElement {
    private bool dragging = false;
    private bool mouseDown = false;
    public float sizeX { get; private set; }
    public float sizeY { get; private set; }

    public DraggableElement() {

        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<MouseUpEvent>(OnMouseUp);
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
        RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        RegisterCallback<MouseEnterEvent>(OnMouseEnter);

        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt) {
        sizeX = this.resolvedStyle.width;
        sizeY = this.resolvedStyle.height;
    }

    private void OnMouseDown(MouseDownEvent evt) {
        mouseDown = true;
        // Debug.Log("epic");
    }

    private void OnMouseUp(MouseUpEvent evt) {
        mouseDown = false;
        if (dragging) { StopDrag(); }
    }

    private void OnMouseMove(MouseMoveEvent evt) {

        // Debug.Log(evt.mousePosition.x + " ," + evt.mousePosition.y);

        if (mouseDown && dragging == false) { StartDrag(); }

        if (dragging) {
            this.style.left = (evt.mousePosition.x) - sizeX / 2f;
            this.style.top = (evt.mousePosition.y) - sizeY / 2f;
        }

        // Debug.Log(dragging);
    }

    private void OnMouseLeave(MouseLeaveEvent evt) {
        this.style.backgroundColor = Color.black;
        // mouseDown = false;
        // if (dragging) { StopDrag(); }
    }

    private void OnMouseEnter(MouseEnterEvent evt) {
        this.style.backgroundColor = Color.red;
        if (mouseDown == true) { mouseDown = false; }
        if (dragging && mouseDown == false) { StopDrag(); }
    }

    private void StartDrag() {
        BringToFront();
        dragging = true;
    }
    private void StopDrag() { 
        dragging = false;
    }
}
