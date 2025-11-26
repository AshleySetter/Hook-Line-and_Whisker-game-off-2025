using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Tilemap groundTileMap;
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
        DropInventoryFishAtMarket,
        BuyFromMarket,
        GoToBed,
        UnfreezeTime,
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

    public bool IsFacingGroundTile()
    {
        Vector3 inFrontOfPlayer = PlayerMovement.Instance.GetPositionInFrontOfPlayer(2);
        Vector3Int tileMapCell = groundTileMap.WorldToCell(inFrontOfPlayer);
        TileBase tileInFrontOfPlayer = groundTileMap.GetTile(tileMapCell);
        return tileInFrontOfPlayer != null;
    }

    private string GetActionDisplayName(InteractActionType action)
    {
        String actionText = action switch
        {
            InteractActionType.PutDownBucket => "Put Down Bucket",
            InteractActionType.DropFishAtMarket => "Sell Fish",
            InteractActionType.DropInventoryFishAtMarket => "Sell Fish",
            InteractActionType.BuyFromMarket => "Buy from Market",
            InteractActionType.StartFishing => "Start Fishing",
            InteractActionType.StopFishing => "",
            InteractActionType.PickUpBucket => "Pick Up Bucket",
            InteractActionType.PutFishInBucket => "Put Fish in Bucket",
            InteractActionType.StealFromCat => "Steal Fish",
            InteractActionType.GoToBed => "Go to Bed",
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
        if (TutorialPrompt.timeFrozen)
        {
            return InteractActionType.UnfreezeTime;
        }

        // Bucket is held
        if (FishBucket.Instance.IsHeldByPlayer())
        {
            if (FishMarket.Instance.GetWithinInteractDistance())
                if (FishBucket.Instance.GetNumberOfFish() > 0) {
                    return InteractActionType.DropFishAtMarket;
                } else
                {
                    return InteractActionType.BuyFromMarket;
                }
            if (PlayerFishing.Instance.IsFacingWater()) {
                return InteractActionType.None;
            } else
            {
                if (PlayerHome.Instance.GetWithinInteractDistance() && DayNightTimer.Instance.GetDayFinished())
                {
                    return InteractActionType.GoToBed;
                }
                if (IsFacingGroundTile()) {
                    return InteractActionType.PutDownBucket;
                }
            }
        }

        // Not holding bucket
        if (FishMarket.Instance.GetWithinInteractDistance())
            // if have fish
            if (Inventory.Instance.GetNumberOfFish() > 0)
            {
                return InteractActionType.DropInventoryFishAtMarket;
            }
            else
            {
                return InteractActionType.BuyFromMarket;
            }

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

        if (PlayerHome.Instance.GetWithinInteractDistance() && DayNightTimer.Instance.GetDayFinished())
        {
            return InteractActionType.GoToBed;
        }

        return InteractActionType.None;
    }
    
    private void ExecuteInteractAction(InteractActionType action)
    {
        switch (action)
        {
            case InteractActionType.UnfreezeTime:
                TutorialPrompt.UnfreezeTime();
                break;
            case InteractActionType.PutDownBucket:
                FishBucket.Instance.PutDown();
                break;

            case InteractActionType.DropFishAtMarket:
                FishBucket.Instance.TakeAllFish(FishMarket.Instance);
                break;

            case InteractActionType.DropInventoryFishAtMarket:
                Inventory.Instance.TakeAllFish(FishMarket.Instance);
                break;

            case InteractActionType.BuyFromMarket:
                shopUI.SetActive(true);
                EscapeManager.Instance.SetActiveWindow(shopUI);
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
                if (!Inventory.Instance.IsFull()) {
                    closestCat.TakeAllFish(Inventory.Instance);
                    stealCooldownTimer = stealCooldown;
                }
                break;

            case InteractActionType.GoToBed:
                GameManager.Instance.FinishDay();
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
