using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.U2D.Animation;

public class CatAIController : MonoBehaviour, FishContainer
{
    public static readonly List<CatAIController> AllCats = new();
    [SerializeField] Transform target;
    [SerializeField] GameObject visual;
    [SerializeField] Animator animator;
    [SerializeField] GameObject fishVisual;
    [SerializeField] SpriteLibrary spriteLibrary;
    [SerializeField] SpriteLibraryAsset[] catSpriteLibraryAssets;

    private FishSO fish;
    private bool hasStolenFish;
    private Vector3 spawnPoint;
    private bool facingRight;
    NavMeshAgent agent;
    private bool withinInteractDistance;
    private float stealCooldown = 3f;
    private float stealCooldownTimer;

    private void OnEnable() => AllCats.Add(this);
    private void OnDisable() => AllCats.Remove(this);

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        facingRight = true;
        fishVisual.SetActive(false);
        spriteLibrary.spriteLibraryAsset = catSpriteLibraryAssets[UnityEngine.Random.Range(0, catSpriteLibraryAssets.Length)];
        withinInteractDistance = false;
        hasStolenFish = false;
    }

    private void Update()
    {
        Vector3 destination = transform.position;
        if (!hasStolenFish)
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
            destination = spawnPoint;
        }
        if (DayNightTimer.Instance.GetDayFinished())
        {
            // go back home at end of day
            destination = spawnPoint;
        }

        agent.SetDestination(destination);
        HandleMovementAnimation();
        if (stealCooldownTimer > 0)
        {
            stealCooldownTimer -= Time.deltaTime;
        }
    }

    public bool GetWithinInteractDistance()
    {
        return withinInteractDistance;
    }

    public void SetSpawn(Vector3 spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (fish == null)
        {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                if (stealCooldownTimer <= 0 && Inventory.Instance.GetNumberOfFish() > 0)
                {
                    Inventory.Instance.TakeFish(this);
                    stealCooldownTimer = stealCooldown; // when they steal fish - start cooldown
                }
            }
            if (other.TryGetComponent(out FishBucket bucket) && bucket.GetNumberOfFish() > 0)
            {
                bucket.TakeFish(this);
            }
        }
        if (other.TryGetComponent(out PlayerMovement _))
        {
            withinInteractDistance = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            withinInteractDistance = false;
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
        hasStolenFish = true;
    }

    public void TakeAllFish(FishContainer newContainer)
    {
        newContainer.AddFish(fish);
        fish = null;
        fishVisual.SetActive(false);
    }

    public void TakeFish(FishContainer newContainer)
    {
        if (newContainer as Object == Inventory.Instance)
        {
            stealCooldownTimer = stealCooldown; // if player takes fish - start cooldown timer
        }
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
