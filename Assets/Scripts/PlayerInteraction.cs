using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private float stealCooldown = 1f;
    private float stealCooldownTimer;

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
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (FishBucket.Instance.IsHeldByPlayer())
        {
            if (FishMarket.Instance.GetWithinInteractDistance())
            {
                FishBucket.Instance.TakeAllFish(FishMarket.Instance);
            } else {
                FishBucket.Instance.PutDown();
            }
        } else if (FishMarket.Instance.GetWithinInteractDistance())
        {
            Inventory.Instance.TakeAllFish(FishMarket.Instance);
        }
        else
        {
            if (PlayerFishing.Instance.IsFishingAllowed() && !Inventory.Instance.IsFull() && !PlayerFishing.Instance.GetIsFishing())
            {
                PlayerFishing.Instance.StartFishing();
            }
            else if (PlayerFishing.Instance.GetIsFishing())
            {
                PlayerFishing.Instance.StopFishing();
            }
            else if (FishBucket.Instance.GetWithinInteractDistance())
            {
                // if have no fish in inventory - pick up fish bucket
                if (Inventory.Instance.IsEmpty())
                {
                    if (!FishBucket.Instance.IsHeldByPlayer())
                    {
                        FishBucket.Instance.PickUp();
                    }
                }
                else
                {
                    // if have fish in inventory - put fish in bucket
                    Inventory.Instance.TakeAllFish(FishBucket.Instance);
                }
            }
            else
            {
                // Debug.Log("doesn't match any condition");
                CatAIController closestCat = GetClosestCat(this.transform);
                if (closestCat != null && closestCat.GetWithinInteractDistance() && closestCat.IsFull() && stealCooldownTimer <= 0)
                {
                    closestCat.TakeAllFish(Inventory.Instance);
                    stealCooldownTimer = stealCooldown;
                }
            }
        }
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
