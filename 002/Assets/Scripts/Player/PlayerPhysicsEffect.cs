using System.Collections;
using UnityEngine;

public class PlayerPhysicsEffect : MonoBehaviour
{
    public float knockBackDistance = 5f;
    public float knockBackTime = 2f;
    public float knockBackDuration = 0.5f;
    public float timer = 0f;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public IEnumerator SmoothKnockback(Direction direction)
    {
        float knockbackForce = direction == Direction.Left ? -knockBackDistance : knockBackDistance;

        while (timer < knockBackDuration)
        {
            float progress = timer / knockBackDuration;
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);
            float currentForce = Mathf.Lerp(knockbackForce, 0f, easedProgress);

            rb.linearVelocity = new Vector2(currentForce, rb.linearVelocity.y);

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        timer = 0f;
    }
}
