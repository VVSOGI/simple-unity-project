using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 moveVector;
    private InputAction moveAction;
    public float moveSpeed = 5;

    private void Start()
    {
        // Start
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        // Update
        moveVector = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(moveVector.x, moveVector.y, 0f) * Time.fixedDeltaTime * moveSpeed;
    }
}