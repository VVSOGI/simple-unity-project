using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public string playerName = "Benny";
    public float moveSpeed = 2.5f;
    private float vectorX = 0;

    public Rigidbody2D rb;
    private InputAction inputAction;

    void Start()
    {
        inputAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        Vector2 moveVector = inputAction.ReadValue<Vector2>();
        vectorX = NormalizeVectorX(moveVector.x);
        rb.linearVelocity = new Vector2(vectorX * moveSpeed, rb.linearVelocity.y);

        if (vectorX > 0)
        {
            ChangeFaceRight();
        }

        if (vectorX < 0)
        {
            ChangeFaceLeft();
        }
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
}
