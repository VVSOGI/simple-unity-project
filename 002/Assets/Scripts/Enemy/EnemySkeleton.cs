using UnityEngine;

public class EnemySkeleton : Enemy
{
    public float moveSpeed = 2f;

    private float vectorX = 0;
    private Rigidbody2D rb;


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
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }
}
