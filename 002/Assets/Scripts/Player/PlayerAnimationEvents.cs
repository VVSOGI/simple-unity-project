using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player player;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera mainCamera;

    public void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void AttemptAttack()
    {
        Debug.Log("Attack!");
    }

    public void DisabledJumpAndMovement()
    {
        AttackTowardsMouse();
        player.changeJumpAndMovementState(false);
    }

    public void enabledJumpAndMovement()
    {
        player.changeJumpAndMovementState(true);
    }


    public void AttackTowardsMouse()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = playerTransform.position.z;

        Vector3 direction = (mouseWorldPos - playerTransform.position).normalized;

        if (direction.x > 0) player.ChangeFaceRight();
        if (direction.x < 0) player.ChangeFaceLeft();
    }
}
