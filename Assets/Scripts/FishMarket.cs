using System;
using UnityEngine;

public class FishMarket : MonoBehaviour, FishContainer
{
    public static FishMarket Instance { get; private set; }

    public event Action OnCoinsChanged;
    private int coins;
    private FishSO[] fish;
    private bool withinInteractDistance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        coins = 0;
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

    public int GetCoins()
    {
        return coins;
    }

    public void AddFish(FishSO fish)
    {
        coins += fish.fishValue;
        OnCoinsChanged?.Invoke();
    }

    public FishSO[] GetFish()
    {
        return fish;
    }

    public int GetNumberOfFish()
    {
        return fish.Length;
    }

    public bool IsFull()
    {
        return false;
    }

    public void TakeAllFish(FishContainer newContainer)
    {
        throw new System.NotImplementedException();
    }

    public void TakeFish(FishContainer newContainer)
    {
        throw new System.NotImplementedException();
    }
}
