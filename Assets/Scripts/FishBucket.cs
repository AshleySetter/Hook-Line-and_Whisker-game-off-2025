using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class FishBucket : MonoBehaviour, FishContainer
{
    public static FishBucket Instance { get; private set; }
    [SerializeField] private AudioClip coinGet;
    private List<FishSO> fishInBucket;
    private bool withinInteractDistance;
    private bool heldByPlayer;

    private void Awake()
    {
        Instance = this;
        fishInBucket = new List<FishSO>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
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

    public bool GetWithinInteractDistance()
    {
        return withinInteractDistance;
    }

    public int GetNumberOfFish()
    {
        return fishInBucket.Count;
    }

    public bool IsFull()
    {
        return false;
    }

    public void AddFish(FishSO fish)
    {
        fishInBucket.Add(fish);
    }

    public void TakeAllFish(FishContainer newContainer)
    {
        FishSO[] fishes = fishInBucket.ToArray();
        int fishRemoved = 0;
        for (int i = 0; i < fishes.Length; i++)
        {
            int indexForCallbacks = i;
            if (newContainer.IsFull())
            {
                break;
            }
            newContainer.AddFish(fishes[i]);
            if (newContainer is FishMarket)
            {
                StartCoroutine(DoAfterDelayUtility.DoAfterDelay(i * 0.5f, () =>
                {
                    // play fish transfer visual / sound fx
                    SoundFXManager.Instance.PlaySoundFXClip(coinGet, this.transform, 1, 1 + 0.5f * indexForCallbacks);
                }));
            }
            fishRemoved++;
        }
        fishInBucket.RemoveRange(0, fishRemoved);
    }

    public void TakeFish(FishContainer newContainer)
    {
        if (fishInBucket.Count > 0) {
            int fishIndexTaken = UnityEngine.Random.Range(0, fishInBucket.Count - 1);
            FishSO fishTaken = fishInBucket[fishIndexTaken];
            fishInBucket.RemoveAt(fishIndexTaken);
            newContainer.AddFish(fishTaken);
        } else
        {
            Debug.LogError("Tried to take fish from empty fish bucket");
        }
    }

    public FishSO[] GetFish()
    {
        return fishInBucket.ToArray();
    }

    public void PickUp()
    {
        transform.parent = PlayerMovement.Instance.GetBucketCarryPoint();
        transform.localPosition = Vector3.zero;
        heldByPlayer = true;
    }

    public void PutDown()
    {
        transform.parent = null;
        transform.position = PlayerMovement.Instance.GetPositionInFrontOfPlayer();
        heldByPlayer = false;
    }
    
    public bool IsHeldByPlayer()
    {
        return heldByPlayer;
    }
}
