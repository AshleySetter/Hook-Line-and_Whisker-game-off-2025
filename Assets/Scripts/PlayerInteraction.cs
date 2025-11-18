using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (FishBucket.Instance.IsHeldByPlayer())
        {
            FishBucket.Instance.PutDown();
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
            }
        }
    }
}
