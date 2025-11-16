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
        Debug.Log("interact action");
        if (FishBucket.Instance.IsHeldByPlayer())
        {
            Debug.Log("put down bucket");
            FishBucket.Instance.PutDown();
        }
        else
        {
            if (PlayerFishing.Instance.IsFishingAllowed() && !Inventory.Instance.IsFull() && !PlayerFishing.Instance.GetIsFishing())
            {
                Debug.Log("start fishing");
                PlayerFishing.Instance.StartFishing();
            }
            else if (PlayerFishing.Instance.GetIsFishing())
            {
                Debug.Log("stop fishing");
                PlayerFishing.Instance.StopFishing();
            }
            else if (FishBucket.Instance.GetWithinInteractDistance())
            {
                Debug.Log("fish bucket within interact distance");
                // if have no fish in inventory - pick up fish bucket
                if (Inventory.Instance.IsEmpty())
                {
                    if (!FishBucket.Instance.IsHeldByPlayer())
                    {
                        Debug.Log("pick up bucket");
                        FishBucket.Instance.PickUp();
                    }
                }
                else
                {
                    // if have fish in inventory - put fish in bucket
                    Debug.Log("put inventory fish in bucket");
                    Inventory.Instance.TakeAllFish(FishBucket.Instance);
                }
            }
            else
            {
                Debug.Log("doesn't match any condition");
            }
        }
    }
}
