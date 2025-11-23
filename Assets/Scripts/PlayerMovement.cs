using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject bucketCarryPoint;

    private float walkSpeed = 3;
    private Vector3 outsideFrontDoorPosition = new Vector3(3.63f, 0.37f, 0f);
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
            animator.SetBool("IsMoving", false);
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
            bucketCarryPoint.transform.localPosition = new Vector3(-0.1f, 0, 0);
        }
        else if (rigidBody.linearVelocity.y > 0)
        {
            animator.SetBool("FacingSideways", false);
            animator.SetBool("FacingAway", true);
            animator.SetBool("FacingCamera", false);
            facingVector = new Vector3(0, 1, 0);
            bucketCarryPoint.transform.localPosition = new Vector3(0, -0.3f, 0);
        }
        else if (rigidBody.linearVelocity.y < 0)
        {
            animator.SetBool("FacingSideways", false);
            animator.SetBool("FacingAway", false);
            animator.SetBool("FacingCamera", true);
            facingVector = new Vector3(0, -1, 0);
            bucketCarryPoint.transform.localPosition = Vector3.zero;
        }
    }

    public void MovePlayerToFrontDoor()
    {
        player.transform.position = outsideFrontDoorPosition;
        animator.SetBool("FacingSideways", false);
        animator.SetBool("FacingAway", false);
        animator.SetBool("FacingCamera", true);
    }

    public GameObject GetVisual()
    {
        return visual;
    }

    public Transform GetBucketCarryPoint()
    {
        return bucketCarryPoint.transform;
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
    
    public Vector3 GetPositionInFrontOfPlayer()
    {
        Vector3 inFrontOfPlayer = this.transform.position + GetFacingVector();
        return inFrontOfPlayer;
    }
}
