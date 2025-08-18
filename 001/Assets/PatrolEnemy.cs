using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction
{
    Left,
    Right
}

public class PatrolEnemy : MonoBehaviour
{
    public float health = 3;
    public float moveSpeed = 2f;
    public float distance = 0.6f;
    public float attackRange = 10f;
    public float retrieveDistance = 2.5f;
    public float chaseSpeed = 4f;
    public float attackRadius = 1f;
    public bool inRange = false;

    public Transform checkPoint;
    public Transform player;
    public Transform AttackPoint;
    public LayerMask layerMask;
    public LayerMask attackLayerMask;
    public Animator animator;

    private GameManager gameManager;
    private Direction characterDirection = Direction.Left;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        gameManager = FindFirstObjectByType<GameManager>();

    }


    void Update()
    {
        if (!gameManager.isGameActive)
        {
            animator.SetBool("Idle", true);
            return;
        }


        if (health <= 0)
        {
            Died();
            return;
        }

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }


        if (inRange)
        {
            if (player.position.x > transform.position.x && characterDirection == Direction.Left)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                characterDirection = Direction.Right;
            }

            if (player.position.x < transform.position.x && characterDirection == Direction.Right)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                characterDirection = Direction.Left;
            }


            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack1", true);
            }

            return;
        }

        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

        if (!hit && characterDirection == Direction.Left)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            characterDirection = Direction.Right;
        }

        else if (!hit && characterDirection == Direction.Right)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            characterDirection = Direction.Left;
        }
    }


    public void Died()
    {
        Destroy(this.gameObject);
    }


    public void GetDamaged(int damage)
    {
        if (health == 0)
        {
            return;
        }

        animator.SetTrigger("Hurt");
        health -= damage;
    }


    public void Attack()
    {
        Collider2D colliderInfo = Physics2D.OverlapCircle(AttackPoint.position, attackRadius, attackLayerMask);

        if (colliderInfo)
        {
            Player player = colliderInfo.gameObject.GetComponent<Player>();

            if (player)
            {
                player.GetDamage(1);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (!checkPoint)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (!AttackPoint) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRadius);
    }
}
