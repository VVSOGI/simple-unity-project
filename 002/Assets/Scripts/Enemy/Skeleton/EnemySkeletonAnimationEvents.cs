using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySkeletonAnimationEvents : MonoBehaviour
{
    private EnemySkeleton enemySkeleton;

    [SerializeField] private EnemySkeletonAttack enemySkeletonAttack;
    [SerializeField] private Transform enemySkeletonTransform;

    public void AttemptAttack()
    {
        enemySkeletonAttack.AttemptAttack();
    }

    private void Awake()
    {
        enemySkeleton = GetComponentInParent<EnemySkeleton>();
    }

    private void DisabledJumpAndMovement()
    {
        enemySkeleton.StopMove();
    }

    private void enabledJumpAndMovement()
    {
        enemySkeleton.StartMove();
    }

}
