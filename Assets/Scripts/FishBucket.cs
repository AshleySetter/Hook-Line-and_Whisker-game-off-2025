using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class FishBucket : MonoBehaviour, FishContainer
{
    public static FishBucket Instance { get; private set; }
    public event Action OnFishCountChanged;
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
        OnFishCountChanged?.Invoke();
    }

    public void TakeAllFish(FishContainer newContainer)
    {
        FishSO[] fishes = fishInBucket.ToArray();
        int fishRemoved = 0;
        for (int i = 0; i < fishes.Length; i++)
        {
            int indexForCallbacks = i;
            FishSO fishForCallbacks = fishes[i];
            if (newContainer.IsFull())
            {
                break;
            }
            if (newContainer is FishMarket)
            {
                StartCoroutine(DoAfterDelayUtility.DoAfterDelay(i * 0.5f, () =>
                {
                    // play fish transfer visual / sound fx
                    SoundFXManager.Instance.PlaySoundFXClip(coinGet, this.transform, 1, 1 + 0.5f * indexForCallbacks);
                    newContainer.AddFish(fishForCallbacks);
                }));
            } else
            {
                newContainer.AddFish(fishes[i]);
            }
            fishRemoved++;
        }
        fishInBucket.RemoveRange(0, fishRemoved);
        OnFishCountChanged?.Invoke();
    }

    public void TakeFish(FishContainer newContainer)
    {
        if (fishInBucket.Count > 0) {
            int fishIndexTaken = UnityEngine.Random.Range(0, fishInBucket.Count - 1);
            FishSO fishTaken = fishInBucket[fishIndexTaken];
            fishInBucket.RemoveAt(fishIndexTaken);
            newContainer.AddFish(fishTaken);
            OnFishCountChanged?.Invoke();
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
