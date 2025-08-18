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
    public float dropDistance = 2f;

    [SerializeField] private LayerMask checkLayerMask;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private LayerMask dropLayerMask;
    [SerializeField] private Transform dropPoint;

    private Rigidbody2D rb;
    private Direction facing = Direction.Right;
    private Animator animator;

    public void ChangeFaceLeft()
    {
        transform.eulerAngles = new Vector3(0, -180f, 0);
    }

    public void ChangeFaceRight()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (isDeath)
        {
            animator.SetBool("Death", true);
            return;
        }

        if (facing == Direction.Right)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }

        if (facing == Direction.Left)
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        }

        handleFacing();
        handleDropCheck();
        handleAnimation();
    }

    private void handleAnimation()
    {
        animator.SetFloat("Move", 1);
    }

    private void handleDropCheck()
    {
        RaycastHit2D checkGround = Physics2D.Raycast(dropPoint.position, Vector2.down, dropDistance, dropLayerMask);

        if (!checkGround && facing == Direction.Right)
        {
            ChangeFaceLeft();
            facing = Direction.Left;
            return;
        }

        if (!checkGround && facing == Direction.Left)
        {
            ChangeFaceRight();
            facing = Direction.Right;
            return;
        }
    }

    private void handleFacing()
    {
        RaycastHit2D checkWall = Physics2D.Raycast(checkPoint.position, Vector2.down, checkDistance, checkLayerMask);

        if (checkWall && facing == Direction.Right)
        {
            ChangeFaceLeft();
            facing = Direction.Left;
            return;
        }

        if (checkWall && facing == Direction.Left)
        {
            ChangeFaceRight();
            facing = Direction.Right;
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * checkDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(dropPoint.position, Vector2.down * dropDistance);
    }
}
