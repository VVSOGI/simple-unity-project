using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpForce = 5;
    public float jumpDuration = 0.2f;
    public bool isCanJump = true;

    public Rigidbody2D rigidBody2D;
    public Animator animator;

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
            animator.SetBool("Jump", true);
            StartCoroutine(EndJumpAnimation(jumpDuration));

        }

        if (Math.Abs(moveVector.x) > 0.1f)
        {
            animator.SetFloat("Run", 1);
        }

        if (Math.Abs(moveVector.x) < 0.1f)
        {
            animator.SetFloat("Run", 0);
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


    private IEnumerator EndJumpAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("Jump", false);
    }
}
