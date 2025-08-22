using System.Collections;
using UnityEngine;

public class PlayerPhysicsEffect : MonoBehaviour
{
    [Header("KnockBack")]
    public float knockBackDistance = 40f;
    public float knockBackTime = 2f;
    public float knockBackDuration = 0.5f;
    public float knockBackTimer = 0f;

    [Header("Quick Jump Dash")]
    public float jumpDashDistance = 40f;
    public float jumpDashTime = 2f;
    public float jumpDashDuration = 0.5f;
    public float jumpDashTimer = 0f;


    [Header("RigidBody2D")]
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public IEnumerator QuickJumpDash(Direction direction)
    {
        float jumpDashForce = direction == Direction.Left ? -jumpDashDistance : jumpDashDistance;

        while (jumpDashTimer < jumpDashDuration)
        {
            float progress = jumpDashTimer / jumpDashDuration;
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);
            float currentForce = Mathf.Lerp(jumpDashForce, 0f, easedProgress);

            rb.linearVelocity = new Vector2(currentForce, rb.linearVelocity.y);

            jumpDashTimer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        jumpDashTimer = 0f;
    }

    public IEnumerator SmoothKnockback(Direction direction)
    {
        float knockbackForce = direction == Direction.Left ? -knockBackDistance : knockBackDistance;

        while (knockBackTimer < knockBackDuration)
        {
            float progress = knockBackTimer / knockBackDuration;
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);
            float currentForce = Mathf.Lerp(knockbackForce, 0f, easedProgress);

            rb.linearVelocity = new Vector2(currentForce, rb.linearVelocity.y);

            knockBackTimer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        knockBackTimer = 0f;
    }
}
