using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;
    private float walkSpeed = 3;
    private bool facingRight;

    private void Update()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalised();
        rigidBody.linearVelocity = inputVector * walkSpeed;
        if (rigidBody.linearVelocity.magnitude > 0)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
        if (Mathf.Abs(rigidBody.linearVelocity.x) > 0)
        {
            if (rigidBody.linearVelocity.x > 0 && facingRight)
            {
                FlipPlayer();
            }
            else if (rigidBody.linearVelocity.x < 0 && !facingRight)
            {
                FlipPlayer();
            }
            animator.SetBool("FacingSideways", true);
            animator.SetBool("FacingAway", false);
            animator.SetBool("FacingCamera", false);
        }
        else if (rigidBody.linearVelocity.y > 0)
        {
            animator.SetBool("FacingSideways", false);
            animator.SetBool("FacingAway", true);
            animator.SetBool("FacingCamera", false);
        }
        else if (rigidBody.linearVelocity.y < 0)
        {
            animator.SetBool("FacingSideways", false);
            animator.SetBool("FacingAway", false);
            animator.SetBool("FacingCamera", true);
        }
    }
    
    private void FlipPlayer()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
