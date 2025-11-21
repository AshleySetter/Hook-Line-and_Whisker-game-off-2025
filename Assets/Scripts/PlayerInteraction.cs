using System;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactText;
    private float stealCooldown = 1f;
    private float stealCooldownTimer;

    public enum InteractActionType {
        None,
        PutDownBucket,
        DropFishAtMarket,
        PickUpBucket,
        PutFishInBucket,
        StartFishing,
        StopFishing,
        StealFromCat,
        TakeFishFromMarket
    }


    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void Update()
    {
        if (stealCooldownTimer > 0)
        {
            stealCooldownTimer -= Time.deltaTime;
        }
        var action = GetInteractAction();
        interactText.text = GetActionDisplayName(action);
    }

    private string GetActionDisplayName(InteractActionType action)
    {
        String actionText = action switch
        {
            InteractActionType.PutDownBucket => "Put Down Bucket",
            InteractActionType.DropFishAtMarket => "Sell Fish",
            InteractActionType.TakeFishFromMarket => "Take Fish",
            InteractActionType.StartFishing => "Start Fishing",
            InteractActionType.StopFishing => "",
            InteractActionType.PickUpBucket => "Pick Up Bucket",
            InteractActionType.PutFishInBucket => "Put Fish in Bucket",
            InteractActionType.StealFromCat => "Steal Fish",
            _ => ""
        };
        String interactText = "";
        if (actionText != "") {
            interactText = $"Press E to {actionText}";
        }
        return interactText;
    }

    private InteractActionType GetInteractAction()
    {
        // Bucket is held
        if (FishBucket.Instance.IsHeldByPlayer())
        {
            if (FishMarket.Instance.GetWithinInteractDistance())
                return InteractActionType.DropFishAtMarket;

            return InteractActionType.PutDownBucket;
        }

        // Not holding bucket
        if (FishMarket.Instance.GetWithinInteractDistance())
            return InteractActionType.TakeFishFromMarket;

        // Fishing logic
        if (PlayerFishing.Instance.IsFishingAllowed() && 
            !Inventory.Instance.IsFull() && 
            !PlayerFishing.Instance.GetIsFishing())
            return InteractActionType.StartFishing;

        if (PlayerFishing.Instance.GetIsFishing())
            return InteractActionType.StopFishing;

        // Bucket interactions
        if (FishBucket.Instance.GetWithinInteractDistance())
        {
            if (Inventory.Instance.IsEmpty())
                return InteractActionType.PickUpBucket;

            return InteractActionType.PutFishInBucket;
        }

        // Cat logic
        CatAIController closestCat = GetClosestCat(this.transform);
        if (closestCat != null &&
            closestCat.GetWithinInteractDistance() &&
            closestCat.IsFull() &&
            stealCooldownTimer <= 0)
            return InteractActionType.StealFromCat;

        return InteractActionType.None;
    }
    
    private void ExecuteInteractAction(InteractActionType action)
    {
        switch (action)
        {
            case InteractActionType.PutDownBucket:
                FishBucket.Instance.PutDown();
                break;

            case InteractActionType.DropFishAtMarket:
                FishBucket.Instance.TakeAllFish(FishMarket.Instance);
                break;

            case InteractActionType.TakeFishFromMarket:
                Inventory.Instance.TakeAllFish(FishMarket.Instance);
                break;

            case InteractActionType.StartFishing:
                PlayerFishing.Instance.StartFishing();
                break;

            case InteractActionType.StopFishing:
                PlayerFishing.Instance.StopFishing();
                break;

            case InteractActionType.PickUpBucket:
                FishBucket.Instance.PickUp();
                break;

            case InteractActionType.PutFishInBucket:
                Inventory.Instance.TakeAllFish(FishBucket.Instance);
                break;

            case InteractActionType.StealFromCat:
                CatAIController closestCat = GetClosestCat(this.transform);
                closestCat.TakeAllFish(Inventory.Instance);
                stealCooldownTimer = stealCooldown;
                break;

            case InteractActionType.None:
            default:
                break;
        }
    }


    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        var action = GetInteractAction();
        ExecuteInteractAction(action);
    }

    private CatAIController GetClosestCat(Transform player)
    {
        CatAIController closest = null;
        float minDist = Mathf.Infinity;

        foreach (var cat in CatAIController.AllCats)
        {
            float dist = (cat.transform.position - player.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = cat;
            }
        }

        return closest;
    }
}
