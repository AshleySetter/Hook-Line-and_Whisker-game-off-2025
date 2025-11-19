using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour, FishContainer
{
    public static Inventory Instance { get; private set; }
    private List<FishSO> fishInInventory;
    private int capacity = 5;
    [SerializeField] private AudioClip fishTransferSound;
    public event Action OnInventoryChanged;

    private void Awake()
    {
        Instance = this;
        fishInInventory = new List<FishSO>();
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
            if (newContainer.IsFull())
            {
                break;
            }
            newContainer.AddFish(fishes[i]);
            StartCoroutine(DoAfterDelayUtility.DoAfterDelay(i * 0.5f, () =>
            {
                // play fish transfer visual / sound fx
                SoundFXManager.Instance.PlaySoundFXClip(fishTransferSound, this.transform, 1, 1 + 0.5f * i);
            }));
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
