using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player player;

    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera mainCamera;

    public void PlaySound()
    {
        playerAttack.PlayAttackSound();
    }

    public void AttemptAttack()
    {
        playerAttack.DamageEnemies();
    }

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void DisabledJumpAndMovement()
    {
        AttackTowardsMouse();
        player.changeJumpAndMovementState(false);
    }

    private void enabledJumpAndMovement()
    {
        player.changeJumpAndMovementState(true);
    }

    private void AttackTowardsMouse()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = playerTransform.position.z;

        Vector3 direction = (mouseWorldPos - playerTransform.position).normalized;

        if (direction.x > 0)
        {
            player.ChangeFaceRight();
            player.facing = Direction.Right;
        }
        if (direction.x < 0)
        {
            player.ChangeFaceLeft();
            player.facing = Direction.Left;
        }
    }
}
