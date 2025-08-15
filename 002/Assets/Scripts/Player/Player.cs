using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public string playerName = "Benny";
    public float moveSpeed = 8f;
    public bool canMove = true;

    public Rigidbody2D rb;
    public Animator animator;
    public PlayerJump playerJump;

    private float vectorX = 0;
    private InputAction inputAction;

    public void changeJumpAndMovementState(bool state)
    {
        canMove = state;
        playerJump.canJump = state;
    }

    public void ChangeFaceLeft()
    {
        transform.eulerAngles = new Vector3(0, -180f, 0);
    }

    public void ChangeFaceRight()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void Start()
    {
        inputAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        Vector2 moveVector = inputAction.ReadValue<Vector2>();
        vectorX = NormalizeVectorX(moveVector.x);

        if (!canMove)
        {
            rb.linearVelocity = new Vector2(vectorX * moveSpeed / 4, rb.linearVelocity.y);
            return;
        }

        if (canMove)
        {
            rb.linearVelocity = new Vector2(vectorX * moveSpeed, rb.linearVelocity.y);

        }

        if (vectorX > 0)
        {
            ChangeFaceRight();
        }

        if (vectorX < 0)
        {
            ChangeFaceLeft();
        }

        if (Math.Abs(moveVector.x) > 0.1f)
        {
            animator.SetFloat("Move", 1);
        }

        if (Math.Abs(moveVector.x) < 0.1f)
        {
            animator.SetFloat("Move", 0);
        }
    }

    private float NormalizeVectorX(float vectorX)
    {
        if (vectorX > 0)
        {
            return 1f;
        }
        else if (vectorX < 0)
        {
            return -1f;
        }

        return 0;
    }
}
