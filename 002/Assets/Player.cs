using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public string playerName = "Benny";
    public float moveSpeed = 2.5f;
    public float jumpForce = 6f;

    public Rigidbody2D rigidBody;

    private bool isCanJump = true;
    private float vectorX = 0;
    private InputAction inputAction;

    void Start()
    {
        inputAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        Vector2 moveVector = inputAction.ReadValue<Vector2>();
        vectorX = NormalizeVectorX(moveVector.x);
        rigidBody.linearVelocity = new Vector2(vectorX * moveSpeed, rigidBody.linearVelocity.y);

        if (vectorX > 0)
        {
            ChangeFaceRight();
        }

        if (vectorX < 0)
        {
            ChangeFaceLeft();
        }

        if (Keyboard.current[Key.Space].isPressed && isCanJump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        changeJumpState(false);
    }

    private void changeJumpState(bool state)
    {
        isCanJump = state;
    }

    private float NormalizeVectorX(float vectorX)
    {
        if (vectorX > 0)
        {
            return 1f;
        }
        else if (vectorX < 0)
        {
            return -1f;
        }

        return 0;
    }

    private void ChangeFaceLeft()
    {
        transform.eulerAngles = new Vector3(0, -180f, 0);
    }

    private void ChangeFaceRight()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            changeJumpState(true);
        }
    }
}
