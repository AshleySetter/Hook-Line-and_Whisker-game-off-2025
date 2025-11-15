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
            if (Inventory.Instance.IsEmpty()) {
                // TODO: pick up bucket
                Debug.Log("pick up bucket");
            } else
            {
                // if have fish in inventory - put fish in bucket
                // TODO: transfer fish from inventory to bucket
                Debug.Log("put fish in bucket");
            }
        }
    }
}
