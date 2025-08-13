using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Parameters")]
    public float jumpHeight = 4f;
    public float timeToApex = 0.4f;     // 올라가는 시간
    public float timeToFall = 0.3f;     // 내려오는 시간

    [Header("Advanced Features")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;
    private bool isGrounded;

    // 타이머들
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    // 계산된 물리값들
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

        Debug.Log($"올라갈 때 중력: {upGravity}, 내려올 때 중력: {downGravity}");
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
        // 실시간 물리 계산
        CalculatePhysics();

        // 점프 실행
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);

        Debug.Log($"점프! 높이: {jumpHeight}");
    }

    void ApplyCustomGravity()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.y > 0)
        {
            // 올라갈 때
            rb.linearVelocity += Vector2.up * upGravity * Time.deltaTime;

            // 가변 점프: 키를 떼면 빠른 하강
            if (!Keyboard.current[Key.Space].isPressed)
            {
                rb.linearVelocity += Vector2.up * upGravity * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
        else if (velocity.y < 0)
        {
            // 내려올 때
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
}
