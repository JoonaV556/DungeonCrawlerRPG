using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Perceivable : MonoBehaviour 
{
    public Transform transform { get; protected set; }

    private void Start() {
        transform = GetComponent<Transform>();
    }
}

