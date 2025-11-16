using UnityEngine;
using UnityEngine.AI;

public class CatAIController : MonoBehaviour, FishContainer
{
    [SerializeField] Transform target;
    [SerializeField] GameObject visual;
    [SerializeField] Animator animator;
    [SerializeField] GameObject fishVisual;
    private FishSO fish;
    
    private bool facingRight;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        facingRight = true;
        fishVisual.SetActive(false);
    }

    private void Update()
    {
        Vector3 destination = transform.position;
        if (fish == null)
        {
            if (FishBucket.Instance.GetNumberOfFish() > 0)
            {
                destination = FishBucket.Instance.transform.position;
            }
            else if (Inventory.Instance.GetNumberOfFish() > 0)
            {
                destination = PlayerMovement.Instance.transform.position;
            }
        }
        else
        {
            // go back home with fish
            destination = Vector3.zero;
        }

        agent.SetDestination(destination);
        HandleMovementAnimation();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (fish == null)
        {
            Debug.Log("on trigger enter");
            if (other.TryGetComponent(out PlayerMovement player))
            {
                Debug.Log("player within distance of Cat");
                Inventory.Instance.TakeFish(this);
            }
            if (other.TryGetComponent(out FishBucket bucket))
            {
                Debug.Log("bucket within distance of Cat");
                bucket.TakeFish(this);
            }
        }
    }

    private void HandleMovementAnimation()
    {
        if (agent.velocity.x < 0)
        {
            animator.SetBool("IsRunning", true);
            if (facingRight)
            {
                FlipVisual();
            }
        }
        else if (agent.velocity.x > 0)
        {
            animator.SetBool("IsRunning", true);
            if (!facingRight)
            {
                FlipVisual();
            }
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    private void FlipVisual()
    {
        facingRight = !facingRight;
        visual.transform.localScale = new Vector3(visual.transform.localScale.x * -1, visual.transform.localScale.y, visual.transform.localScale.z);
    }

    public int GetNumberOfFish()
    {
        if (fish != null)
        {
            return 1;
        } else
        {
            return 0;
        }
    }

    public bool IsFull()
    {
        if (fish != null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void AddFish(FishSO fish)
    {
        this.fish = fish;
        fishVisual.SetActive(true);
    }

    public void TakeAllFish(FishContainer newContainer)
    {
        newContainer.AddFish(fish);
        fish = null;
        fishVisual.SetActive(false);
    }

    public void TakeFish(FishContainer newContainer)
    {
        newContainer.AddFish(fish);
        fish = null;
        fishVisual.SetActive(false);
    }

    public FishSO[] GetFish()
    {
        FishSO[] fishes = { fish };
        return fishes;
    }
}
