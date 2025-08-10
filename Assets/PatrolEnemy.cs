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
    public Transform checkPoint;
    public LayerMask layerMask;

    private Direction characterDirection = Direction.Left;

    void Start()
    {

    }


    void Update()
    {
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
    }


}
