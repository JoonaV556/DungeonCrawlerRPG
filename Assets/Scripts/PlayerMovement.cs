using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    #region Properties
    [SerializeField]
    private float MovementSpeed = 20f;

    [SerializeField]
    private Rigidbody2D Rigidbody;

    [SerializeField]
    private ContactFilter2D ContactFilter;

    private Vector2 InputVector = new Vector2(0, 0);
    private List<RaycastHit2D> RaycastHits = new List<RaycastHit2D>(); // Used for storing movement preventing collisions
    public float collisionOffset = 0.15f; // How far movement raycast collisions are cast in addition to characters base movement speed
    #endregion

    private void FixedUpdate() {

        if (InputVector != Vector2.zero) {
            bool success = TryMove(InputVector); // Try to move
            if (!success) { 
                success = TryMove(new Vector2(InputVector.x, 0)); // If collided with something, try to slide along X axis
                if (!success) {
                    TryMove(new Vector2(0, InputVector.y)); // If collided with something along x, try to slide along Y axis
                }
            }
        }
    }

    private bool TryMove(Vector2 direction) {
        int collisionCount = Rigidbody.Cast(
            direction,
            ContactFilter,
            RaycastHits,
            MovementSpeed * Time.fixedDeltaTime + collisionOffset);

        if (collisionCount == 0)
        {
            Rigidbody.MovePosition(Rigidbody.position + direction * MovementSpeed * Time.fixedDeltaTime);
            return true;
        } else {
            return false;
        }
    }

    public void OnMove(InputValue input) {
        // ("dick " + input.Get<Vector2>());
        InputVector = input.Get<Vector2>();
    }
}
