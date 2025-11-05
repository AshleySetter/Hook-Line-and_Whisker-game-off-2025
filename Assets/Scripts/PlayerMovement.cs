using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    private float walkSpeed = 5;

    private void Update()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalised();

        rigidBody.linearVelocity = inputVector * walkSpeed;
        Debug.Log(rigidBody.linearVelocity);
    }
}
