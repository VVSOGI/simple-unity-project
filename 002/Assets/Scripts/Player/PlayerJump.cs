using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Parameters")]
    public float jumpHeight = 3.5f;
    public float timeToApex = 0.3f;
    public float timeToFall = 0.3f;

    [Header("Advanced Features")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;
    private bool isGrounded;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private float upGravity;
    private float downGravity;
    private float jumpVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        CalculatePhysics();
    }

    void CalculatePhysics()
    {
        upGravity = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        downGravity = -(2 * jumpHeight) / Mathf.Pow(timeToFall, 2);
        jumpVelocity = Mathf.Abs(upGravity) * timeToApex;
    }

    void Update()
    {
        HandleInput();
        HandleCoyoteTime();
        HandleJumpBuffer();
        ApplyCustomGravity();
    }

    void HandleInput()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void HandleCoyoteTime()
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

    void HandleJumpBuffer()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            Jump();
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
    }

    void Jump()
    {
        CalculatePhysics();

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
    }

    void ApplyCustomGravity()
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Vector3 startPos = transform.position;
            Gizmos.color = Color.red;
            Vector3 pos = startPos + jumpHeight * Vector3.up;
            Gizmos.DrawWireSphere(pos, 0.1f);
        }
    }
}
