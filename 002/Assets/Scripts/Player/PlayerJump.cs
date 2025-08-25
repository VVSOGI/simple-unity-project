using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header("Player")]
    public Player player;

    [Header("Player Physics Effect")]
    public PlayerPhysicsEffect playerPhysicsEffect;

    [Header("Jump Parameters")]
    public float jumpHeight = 3.5f;
    public float timeToApex = 0.3f;
    public float timeToFall = 0.3f;
    public bool canJump = true;
    public bool isJumpDash = false;

    [Header("Advanced Features")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float lowJumpMultiplier = 2f;
    public bool isGrounded;

    [Header("Animator")]
    public Animator animator;

    [Header("Effect")]
    public ParticleSystem dustPrefab;

    private Rigidbody2D rb;
    private bool firstJump = false;
    private bool secondJump = false;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private float upGravity;
    private float downGravity;
    private float jumpVelocity;

    IEnumerator KnockbackProcess(Direction direction)
    {
        isJumpDash = true;
        downGravity = 0f;
        animator.SetBool("IsJumpDash", true);
        yield return StartCoroutine(playerPhysicsEffect.QuickJumpDash(direction));
        animator.SetBool("IsJumpDash", false);
        CalculatePhysics();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        CalculatePhysics();
    }

    private void CalculatePhysics()
    {
        upGravity = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        downGravity = -(2 * jumpHeight) / Mathf.Pow(timeToFall, 2);
        jumpVelocity = Mathf.Abs(upGravity) * timeToApex;
    }

    private void Update()
    {
        HandleInput();
        HandleJumpDash();
        HandleCoyoteTime();
        ApplyCustomGravity();
    }

    private void HandleJumpDash()
    {
        if (Keyboard.current[Key.LeftShift].wasPressedThisFrame && !isJumpDash && !isGrounded)
        {
            StartCoroutine(KnockbackProcess(player.facing));
        }
    }

    private void HandleInput()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            jumpBufferCounter = jumpBufferTime;
            if (firstJump && !secondJump)
            {
                Jump();
                secondJump = true;
            }

            if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !firstJump && canJump)
            {
                Jump();
                jumpBufferCounter = 0f;
                coyoteTimeCounter = 0f;
                firstJump = true;
            }
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void HandleCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        CalculatePhysics();
        player.StopFootstepLoop();
        animator.SetTrigger("Jump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
    }

    private void ApplyCustomGravity()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.y > 0)
        {
            rb.linearVelocity += Vector2.up * upGravity * Time.deltaTime;

            if (!Keyboard.current[Key.Space].isPressed)
            {
                rb.linearVelocity += Vector2.up * upGravity * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
        else if (velocity.y <= 0)
        {
            rb.linearVelocity += Vector2.up * downGravity * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("IsGrounded", true);
            isGrounded = true;
            firstJump = false;
            secondJump = false;
            isJumpDash = false;
            PlayDustEffect();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("IsGrounded", false);
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Vector3 startPos = transform.position;
            Gizmos.color = Color.red;
            Vector3 pos = startPos + jumpHeight * Vector3.up;
            Gizmos.DrawWireSphere(pos, 0.1f);
        }
    }

    private void PlayDustEffect()
    {
        ParticleSystem newDust = Instantiate(dustPrefab, transform.position + Vector3.down * 1f, dustPrefab.transform.rotation);
        newDust.Play();
        Destroy(newDust.gameObject, 2f);
    }
}
