using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public string playerName = "Benny";
    public float moveSpeed = 2.5f;

    public Rigidbody2D rigidBody;

    private float vectorX;
    private InputAction inputAction;


    void Start()
    {
        inputAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        Vector2 moveVector = inputAction.ReadValue<Vector2>();
        vectorX = normalizeVectorX(moveVector.x);
        rigidBody.linearVelocity = new Vector2(vectorX * moveSpeed, rigidBody.linearVelocity.y);

        changeFaceDirection(vectorX);
    }

    private float normalizeVectorX(float vectorX)
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

    private void changeFaceDirection(float vectorX)
    {
        if (vectorX > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (vectorX < 0)
        {
            transform.eulerAngles = new Vector3(0, -180f, 0);
        }
    }
}
