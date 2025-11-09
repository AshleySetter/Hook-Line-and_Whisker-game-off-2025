using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFishing : MonoBehaviour
{
    private FishingState fishingState;
    private float timeToCast = 0.5f; // length of cast animation
    private float castingTimer;
    private float timeToHook = 0.5f; // length of hooking fish animation
    private float hookingTimer;
    private float minWaitingTime = 0.3f;
    private float maxWaitingTime = 2f;
    private float waitingTime;
    private float waitingTimer;
    private Vector3 bobberLocation;
    private float bobberMaxAngle = 20;
    private float bobberMinDistance = 2;
    private float bobberMaxDistance = 6;

    public enum FishingState
    {
        NotFishing,
        Casting,
        Waiting,
        HookedFish,
        Fighting,
        Yankable,
        Caught,
    }
    
    public bool IsFishingAllowed()
    {
        // only allow when next to and facing water
        // and when during the fishing part of the day
        return true;
    }

    public bool GetIsFishing()
    {
        return fishingState != FishingState.NotFishing;
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        fishingState = FishingState.NotFishing;
        castingTimer = 0;
        bobberLocation = GetNextBobberLocation();
    }
    
    private Vector3 GetNextBobberLocation()
    {
        float distance = UnityEngine.Random.Range(bobberMinDistance, bobberMaxDistance);
        float angle = Mathf.Deg2Rad * UnityEngine.Random.Range(-bobberMaxAngle, +bobberMaxAngle);
        return new Vector3(Mathf.Cos(Mathf.PI/2 + angle), Mathf.Sin(Mathf.PI/2 + angle), 0);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (IsFishingAllowed() && fishingState == FishingState.NotFishing)
        {
            castingTimer = 0;
            bobberLocation = GetNextBobberLocation();
            // tell player animator to play casting animation
            fishingState = FishingState.Casting;
        }
        else if (fishingState != FishingState.NotFishing)
        {
            fishingState = FishingState.NotFishing;
        }
    }

    private void Update()
    {
        switch (fishingState)
        {
            case FishingState.NotFishing:
                break;
            case FishingState.Casting:
                if (castingTimer > timeToCast)
                {
                    waitingTime = UnityEngine.Random.Range(minWaitingTime, maxWaitingTime);
                    fishingState = FishingState.Waiting;
                }
                castingTimer += Time.deltaTime;
                break;
            case FishingState.Waiting:
                if (waitingTimer > waitingTime)
                {
                    hookingTimer = 0;
                    fishingState = FishingState.HookedFish;
                }
                waitingTimer += Time.deltaTime;
                break;
            case FishingState.HookedFish:
                if (hookingTimer > timeToHook)
                {
                    fishingState = FishingState.Fighting;
                }
                hookingTimer += Time.deltaTime;
                break;
        }
    }
}
