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

    [Header("Animator")]
    public Animator animator;

    [Header("Player")]
    public Player player;

    [Header("Collider")]
    public Collider2D enemyCollider;

    private float timer = 0;

    public void AttemptAttack()
    {
        enemyCollider = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackCheckLayerMask);
    }

    private void Start()
    {
        enemySkeleton = GetComponent<EnemySkeleton>();
        animator = GetComponentInChildren<Animator>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }
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
