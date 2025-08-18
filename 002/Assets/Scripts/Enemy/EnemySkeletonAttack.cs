using UnityEngine;

public class EnemySkeletonAttack : MonoBehaviour
{
    [SerializeField] private float attackRadius = 0.6f;
    [SerializeField] private float dropDistance = 0.6f;
    [SerializeField] private LayerMask attackLayerMask;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask attackCheckLayerMask;
    [SerializeField] private Transform attackCheckPoint;

    [Header("Skeleton")]
    public EnemySkeleton enemySkeleton;

    [Header("Skeleton")]
    public Animator animator;

    private void Start()
    {
        enemySkeleton = GetComponent<EnemySkeleton>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        handleCheckPlayer();
    }

    private void handleCheckPlayer()
    {
        RaycastHit2D checkPlayer = Physics2D.Raycast(attackCheckPoint.position, Vector2.down, dropDistance, attackCheckLayerMask);
        if (checkPlayer)
        {
            animator.SetTrigger("Attack");
            enemySkeleton.StopMove();
        }
        else
        {
            enemySkeleton.StartMove();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(attackCheckPoint.position, Vector2.down * dropDistance);
    }
}
