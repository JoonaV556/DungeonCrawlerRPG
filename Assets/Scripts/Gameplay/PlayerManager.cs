using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // General manager script for player related stuff


    [SerializeField]
    PlayerInput playerInput;

    public void EnableInput() {
        playerInput.ActivateInput();
    }

    public void DisableInput() {
        playerInput.DeactivateInput();
    }  
}
