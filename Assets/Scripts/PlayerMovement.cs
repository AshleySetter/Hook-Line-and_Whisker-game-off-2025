using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject visual;
    private float walkSpeed = 3;
    private bool facingRight;
    private Vector3 facingVector;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (PlayerFishing.Instance.GetIsFishing())
        {
            return;
        }
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
            if (rigidBody.linearVelocity.x > 0)
            {
                facingVector = new Vector3(1, 0, 0);
                if (facingRight)
                {
                    FlipPlayer();
                }
            }
            else if (rigidBody.linearVelocity.x < 0)
            {
                facingVector = new Vector3(-1, 0, 0);
                if (!facingRight)
                {
                    FlipPlayer();
                }
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
            facingVector = new Vector3(0, 1, 0);
        }
        else if (rigidBody.linearVelocity.y < 0)
        {
            animator.SetBool("FacingSideways", false);
            animator.SetBool("FacingAway", false);
            animator.SetBool("FacingCamera", true);
            facingVector = new Vector3(0, -1, 0);
        }
    }

    public Vector3 GetFacingVector()
    {
        return facingVector;
    }
    
    private void FlipPlayer()
    {
        facingRight = !facingRight;
        visual.transform.localScale = new Vector3(visual.transform.localScale.x * -1, visual.transform.localScale.y, visual.transform.localScale.z);
    }
}
