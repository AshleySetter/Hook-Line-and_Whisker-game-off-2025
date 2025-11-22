using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour, FishContainer
{
    public static Inventory Instance { get; private set; }
    private List<FishSO> fishInInventory;
    private int capacity = 1;
    [SerializeField] private AudioClip fishTransferSound;
    [SerializeField] private AudioClip coinSound;
    public event Action OnInventoryChanged;

    private void Awake()
    {
        Instance = this;
        fishInInventory = new List<FishSO>();
    }

    public int GetCapacity()
    {
        return capacity;
    }

    public void AddCapacity()
    {
        capacity += 1;
    }

    public int GetNumberOfFish()
    {
        return fishInInventory.Count;
    }

    public bool IsFull()
    {
        return fishInInventory.Count >= capacity;
    }

    public bool IsEmpty()
    {
        return fishInInventory.Count == 0;
    }

    public void AddFish(FishSO fish)
    {
        fishInInventory.Add(fish);
        OnInventoryChanged?.Invoke();
    }

    public void TakeAllFish(FishContainer newContainer)
    {
        FishSO[] fishes = fishInInventory.ToArray();
        int fishRemoved = 0;
        for (int i = 0; i < fishes.Length; i++)
        {
            int indexForCallbacks = i;
            FishSO fishForCallbacks = fishes[i];
            if (newContainer.IsFull())
            {
                break;
            }
            if (newContainer is FishBucket) {
                StartCoroutine(DoAfterDelayUtility.DoAfterDelay(i * 0.5f, () =>
                {
                    // play fish transfer visual / sound fx
                    SoundFXManager.Instance.PlaySoundFXClip(fishTransferSound, this.transform, 1, 1 + 0.5f * indexForCallbacks);
                    newContainer.AddFish(fishForCallbacks);
                }));
            }
            else if (newContainer is FishMarket)
            {
                StartCoroutine(DoAfterDelayUtility.DoAfterDelay(i * 0.5f, () =>
                {
                    // play fish transfer visual / sound fx
                    SoundFXManager.Instance.PlaySoundFXClip(coinSound, this.transform, 1, 1 + 0.5f * indexForCallbacks);
                    newContainer.AddFish(fishForCallbacks);
                }));
            } else
            {
                newContainer.AddFish(fishes[i]);
            }
            fishRemoved++;
        }
        fishInInventory.RemoveRange(0, fishRemoved);
        OnInventoryChanged?.Invoke();
    }

    public void TakeFish(FishContainer newContainer)
    {
        if (fishInInventory.Count > 0)
        {
            int fishIndexTaken = UnityEngine.Random.Range(0, fishInInventory.Count - 1);
            FishSO fishTaken = fishInInventory[fishIndexTaken];
            fishInInventory.RemoveAt(fishIndexTaken);
            newContainer.AddFish(fishTaken);
            OnInventoryChanged?.Invoke();
        } else
        {
            Debug.LogError("Tried to take fish from empty inventory");
        }
    }

    public FishSO[] GetFish()
    {
        return fishInInventory.ToArray();
    }
}
