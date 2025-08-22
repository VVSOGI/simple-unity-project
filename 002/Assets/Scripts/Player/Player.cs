using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Footstep Audio")]
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepSounds;
    [SerializeField] private float footstepDuration = 0.4f;
    private bool isWalkingSound = false;

    [Header("Basic Attributes")]
    public string playerName = "Benny";
    public float moveSpeed = 8f;
    public bool canMove = true;
    public Direction facing;

    public Rigidbody2D rb;
    public Animator animator;
    public PlayerJump playerJump;
    public PlayerPhysicsEffect playerPhysicsEffect;

    [SerializeField] private int totalHealth = 100;

    private float vectorX = 0;
    private bool isKnockBack = false;
    private bool isDash = false;
    private InputAction inputAction;

    IEnumerator KnockbackProcess(Direction direction)
    {
        isKnockBack = true;
        animator.SetBool("IsKnockBack", true);
        yield return StartCoroutine(playerPhysicsEffect.SmoothKnockback(direction));
        isKnockBack = false;
        animator.SetBool("IsKnockBack", false);

        if (!canMove)
        {
            changeJumpAndMovementState(true);
        }
    }

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

    public void getDamaged(int damage, Direction direction)
    {
        if (totalHealth == 0) return;

        if (direction == Direction.Left)
        {
            StartCoroutine(KnockbackProcess(direction));
        }

        if (direction == Direction.Right)
        {
            StartCoroutine(KnockbackProcess(direction));
        }

        totalHealth -= damage;
    }

    public void StartFootstepLoop()
    {
        if (isWalkingSound) return;
        isWalkingSound = true;
        InvokeRepeating(nameof(PlayRandomFootstep), 0f, footstepDuration);
    }

    public void StopFootstepLoop()
    {
        if (!isWalkingSound) return;
        isWalkingSound = false;
        CancelInvoke(nameof(PlayRandomFootstep));
    }

    private void Start()
    {
        inputAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        handleMove();
        handleFaceDirection();
        handleDash();

        Vector2 moveVector = inputAction.ReadValue<Vector2>();
        vectorX = NormalizeVectorX(moveVector.x);


        if (Math.Abs(moveVector.x) > 0.1f)
        {
            animator.SetFloat("Move", 1);
        }

        if (Math.Abs(moveVector.x) < 0.1f)
        {
            animator.SetFloat("Move", 0);
        }
    }

    private void PlayRandomFootstep()
    {
        if (footstepSounds.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, footstepSounds.Length);
            footstepAudioSource.PlayOneShot(footstepSounds[randomIndex]);
        }
    }

    private void handleFaceDirection()
    {
        if (canMove)
        {
            if (vectorX < 0)
            {
                ChangeFaceLeft();
                facing = Direction.Left;
            }

            if (vectorX > 0)
            {
                ChangeFaceRight();
                facing = Direction.Right;
            }
        }
    }

    private void handleDash()
    {
        if (isDash && playerJump.isGrounded)
        {
            moveSpeed = 12f;
        }
        else
        {
            moveSpeed = 8f;
        }

        if (Keyboard.current[Key.LeftShift].isPressed)
        {
            isDash = true;
        }
        else
        {
            isDash = false;
        }
    }

    private void handleMove()
    {
        if (playerJump.isJumpDash)
        {
            Vector2 velocity = rb.linearVelocity;

            if (Mathf.Abs(vectorX) > 0.1f)
            {
                animator.SetFloat("Move", 1);

                float currentVelocityX = velocity.x;
                float inputDirection = vectorX;

                bool isUsingAcceleration = (currentVelocityX > 0 && inputDirection > 0) ||
                                         (currentVelocityX < 0 && inputDirection < 0);

                if (isUsingAcceleration)
                {
                    float boostForce = inputDirection * moveSpeed * 0.2f;
                    float newVelocityX = currentVelocityX + boostForce * Time.fixedDeltaTime * 60f;

                    rb.linearVelocity = new Vector2(newVelocityX, velocity.y);
                }
            }

            return;
        }

        if (isKnockBack)
        {
            Vector2 velocity = rb.linearVelocity;

            if (Mathf.Abs(vectorX) > 0.1f)
            {
                animator.SetBool("IsKnockBack", false);
                animator.SetFloat("Move", 1);

                float currentVelocityX = velocity.x;
                float inputDirection = vectorX;

                bool isUsingAcceleration = (currentVelocityX > 0 && inputDirection > 0) ||
                                         (currentVelocityX < 0 && inputDirection < 0);

                if (isUsingAcceleration)
                {
                    float boostForce = inputDirection * moveSpeed * 0.2f;
                    float newVelocityX = currentVelocityX + boostForce * Time.fixedDeltaTime * 60f;

                    rb.linearVelocity = new Vector2(newVelocityX, velocity.y);
                }
            }

            return;
        }

        if (!canMove)
        {
            rb.linearVelocity = new Vector2(vectorX * moveSpeed / 4, rb.linearVelocity.y);
            StopFootstepLoop();
            return;
        }

        if (canMove)
        {
            Vector2 moveVector = inputAction.ReadValue<Vector2>();
            vectorX = NormalizeVectorX(moveVector.x);

            if (Math.Abs(moveVector.x) > 0.1f)
            {
                StartFootstepLoop();
            }

            if (Math.Abs(moveVector.x) < 0.1f)
            {
                StopFootstepLoop();
            }

            if (!playerJump.isGrounded)
            {
                StopFootstepLoop();
            }

            rb.linearVelocity = new Vector2(vectorX * moveSpeed, rb.linearVelocity.y);
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
