using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Jump")]
    public PlayerJump playerJump;

    [Header("Collider")]
    public Collider2D[] enemyColliders;

    [SerializeField] private float attackRadius = 0.6f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyMask;

    public void Update()
    {
        HandleInput();
    }

    public void DamageEnemies()
    {
        enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyMask);
    }

    private void HandleInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && playerJump.isGrounded)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        animator.SetTrigger("Attack");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
