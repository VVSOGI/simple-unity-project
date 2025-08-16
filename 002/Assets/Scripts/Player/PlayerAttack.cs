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

    [Header("Basic Attriubes")]
    [SerializeField] private float attackAction1 = 1;

    public void Update()
    {
        HandleInput();
    }

    public void DamageEnemies()
    {
        enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyMask);

        foreach (Collider2D enemy in enemyColliders)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackAction1);
        }
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
