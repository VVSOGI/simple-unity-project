using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpForce = 5;
    public bool isCanJump = true;
    public Rigidbody2D rigidBody2D;

    private Vector2 moveVector;
    private InputAction moveAction;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }


    private void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();

        CalculateFacing();

        if (Keyboard.current[Key.Space].wasPressedThisFrame && isCanJump)
        {
            Jump();
            isCanJump = false;
        }
    }


    private void FixedUpdate()
    {
        transform.position += new Vector3(moveVector.x, 0, 0) * Time.fixedDeltaTime * moveSpeed;
    }


    private void CalculateFacing()
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

    private void Jump()
    {
        rigidBody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isCanJump = true;
        }
    }
}
