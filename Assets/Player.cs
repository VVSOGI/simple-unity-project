using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public int currentCoin = 0;
    public int currentHealth = 3;
    public float moveSpeed = 10f;
    public float jumpForce = 12f;
    public float jumpDuration = 0.3f;
    public float attackRadius = 1.5f;
    public bool isCanJump = true;
    public Text health;
    public Text coinText;

    public Rigidbody2D rigidBody2D;
    public Animator animator;
    public Transform AttackPoint;
    public LayerMask attackLayerMask;

    private Vector2 moveVector;
    private InputAction moveAction;
    private GameManager gameManager;
    private SceneManager sceneManager;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        gameManager = FindFirstObjectByType<GameManager>();
        sceneManager = FindFirstObjectByType<SceneManager>();
    }


    private void Update()
    {
        health.text = currentHealth.ToString();
        coinText.text = currentCoin.ToString();
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


    public void Attack()
    {
        Collider2D colliderInfo = Physics2D.OverlapCircle(AttackPoint.position, attackRadius, attackLayerMask);

        if (colliderInfo)
        {
            PatrolEnemy patrolEnemy = colliderInfo.gameObject.GetComponent<PatrolEnemy>();
            patrolEnemy.GetDamaged(1);
        }
    }


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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.name == "Coin")
        {
            Animator coinAnimator = other.GetComponent<Animator>();
            Debug.Log(coinAnimator.name);
            if (coinAnimator != null && !coinAnimator.GetBool("Collect"))
            {
                currentCoin += 1;
                coinAnimator.SetBool("Collect", true);
                Destroy(other.gameObject, 1f);
            }
        }

        if (other.gameObject.name == "Destination")
        {
            gameManager.isGameActive = false;
            gameManager.isVictory = true;
            sceneManager.LoadLevel(2);
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
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        gameManager.isGameActive = false;
    }


    private void OnDrawGizmosSelected()
    {
        if (!AttackPoint) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRadius);
    }
}
