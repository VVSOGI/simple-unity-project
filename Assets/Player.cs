using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5;
    public Rigidbody2D rigidBody2D;

    private Vector2 moveVector;
    private InputAction moveAction;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }


    private void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();

        this.calculateFacing();
    }


    private void FixedUpdate()
    {
        transform.position += new Vector3(moveVector.x, 0, 0) * Time.fixedDeltaTime * moveSpeed;
    }


    private void calculateFacing()
    {
        if (moveVector.x < 0)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }

        if (moveVector.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
