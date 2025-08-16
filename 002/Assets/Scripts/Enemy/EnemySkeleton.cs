using UnityEngine;

public enum Direction
{
    Left,
    Right
}

public class EnemySkeleton : Enemy
{
    public float moveSpeed = 2f;
    public float checkDistance = 0.6f;

    [SerializeField] private LayerMask checkLayerMask;
    [SerializeField] private Transform checkPoint;

    private Rigidbody2D rb;
    private Direction facing = Direction.Right;

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
        RaycastHit2D checkWall = Physics2D.Raycast(checkPoint.position, Vector2.down, checkDistance, checkLayerMask);

        if (checkWall && facing == Direction.Right)
        {
            ChangeFaceLeft();
            facing = Direction.Left;
        }

        if (checkWall && facing == Direction.Left)
        {
            ChangeFaceRight();
            facing = Direction.Right;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * checkDistance);
    }
}
