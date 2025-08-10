using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int currentHealth = 3;
    public float moveSpeed = 10f;
    public float jumpForce = 12f;
    public float jumpDuration = 0.3f;
    public bool isCanJump = true;
    public Text health;

    public Rigidbody2D rigidBody2D;
    public Animator animator;

    private Vector2 moveVector;
    private InputAction moveAction;


    public void GetDamage(int damage)
    {
        if (currentHealth == 0)
        {
            return;
        }


        if (currentHealth <= 1)
        {
            currentHealth -= damage;
            Die();
            return;
        }

        currentHealth -= damage;
    }


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
        health.text = currentHealth.ToString();
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

        if (Keyboard.current[Key.LeftCtrl].wasPressedThisFrame)
        {
            animator.SetTrigger("Attack");
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
            animator.Play("Idle");
        }
    }


    private IEnumerator EndJumpAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("Jump", false);
    }


    private void Die()
    {
        Debug.Log("Player Died");
    }
}
