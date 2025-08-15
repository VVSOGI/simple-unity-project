using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Jump")]
    public PlayerJump playerJump;


    void Start()
    {

    }

    void Update()
    {
        HandleInput();
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
}
