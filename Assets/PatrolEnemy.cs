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
    public float moveSpeed = 2f;
    public float distance = 0.6f;
    public float attackRange = 10f;
    public float retrieveDistance = 2.5f;
    public float chaseSpeed = 4f;
    public bool inRange = false;

    public Transform checkPoint;
    public Transform player;
    public LayerMask layerMask;
    public Animator animator;

    private Direction characterDirection = Direction.Left;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }


    void Update()
    {
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

    }


}
