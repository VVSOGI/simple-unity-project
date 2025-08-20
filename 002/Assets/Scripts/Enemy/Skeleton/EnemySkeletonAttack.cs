using UnityEngine;

public class EnemySkeletonAttack : MonoBehaviour
{
    [Header("Skeleton")]
    public EnemySkeleton enemySkeleton;

    [Header("Animator")]
    public Animator animator;

    [Header("Collider")]
    public Collider2D playerCollider;

    [SerializeField] private float attackRadius = 0.6f;
    [SerializeField] private float dropDistance = 0.6f;
    [SerializeField] private LayerMask attackLayerMask;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask attackCheckLayerMask;
    [SerializeField] private Transform attackCheckPoint;

    private float timer = 0;
    private int damage = 1;

    public void AttemptAttack()
    {
        playerCollider = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackCheckLayerMask);
        if (playerCollider)
        {
            Direction direction = enemySkeleton.facing;

            Player player = playerCollider.GetComponent<Player>();
            player.getDamaged(damage, direction);
        }
    }

    private void Start()
    {
        enemySkeleton = GetComponent<EnemySkeleton>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        handleCheckPlayer();
    }

    private void handleCheckPlayer()
    {
        RaycastHit2D checkPlayer = Physics2D.Raycast(attackCheckPoint.position, Vector2.down, dropDistance, attackCheckLayerMask);

        if (checkPlayer && enemySkeleton.isCanMove)
        {
            animator.SetTrigger("Attack");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(attackCheckPoint.position, Vector2.down * dropDistance);
    }
}
