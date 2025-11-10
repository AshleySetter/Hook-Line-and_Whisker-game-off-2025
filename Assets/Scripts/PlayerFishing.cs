using System;
using Tweens;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFishing : MonoBehaviour
{
    public static PlayerFishing Instance { get; private set; }

    [SerializeField] private GameObject waterRipples;
    [SerializeField] private GameObject waterSplash;
    [SerializeField] private FishSO[] fishes;

    private FishingState fishingState;
    private float timeToCast = 0.5f; // length of cast animation
    private float castingTimer;
    private float timeToHook = 0.5f; // length of hooking fish animation
    private float hookingTimer;
    private float minWaitingTime = 1.5f;
    private float maxWaitingTime = 5f;
    private float waitingTime;
    private float waitingTimer;
    private Vector3 bobberLocation;
    private float bobberMaxAngle = 20;
    private float bobberMinDistance = 2;
    private float bobberMaxDistance = 5;
    private float fightTime;
    private float fightTimer;
    private FishSO hookedFish;
    private float fishAngle;
    private float fishDistance;
    private float reelStrength = 0.5f;
    private float catchDistance = 1f;

    public enum FishingState
    {
        NotFishing,
        Casting,
        Waiting,
        HookedFish,
        Fighting,
        Reelable,
        Caught,
        Failed
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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnReelAction += GameInput_OnReelAction;
        fishingState = FishingState.NotFishing;
        castingTimer = 0;
        SetNextBobberLocation();
        waterRipples.SetActive(false);
        waterSplash.SetActive(false);
        CatchBar.Instance.gameObject.SetActive(false);
    }

    private void GameInput_OnReelAction(object sender, EventArgs e)
    {
        if (fishingState == FishingState.Reelable)
        {
            if (CatchBar.Instance.GetCatchBarInGreen())
            {
                if (CatchBar.Instance.GetCatchBarInBoost())
                {
                    Debug.Log("Fish Boost reeled!");
                    fishDistance -= reelStrength * 2;
                } else
                {
                    Debug.Log($"Fish reeled");
                    fishDistance -= reelStrength;
                }
                fishDistance -= reelStrength;
                UpdateBobberLocation();
                if (UnityEngine.Random.Range(0, 100) < hookedFish.fightOnReelChance)
                {
                    StartFighting();
                }
                if (fishDistance < catchDistance)
                {
                    fishingState = FishingState.Caught;
                }
            } else
            {
                fishingState = FishingState.Failed;
            }
        }
        else
        {
            if (fishingState != FishingState.Caught || fishingState != FishingState.NotFishing)
            {
                Debug.Log("You reeled at the wrong time!");
                fishingState = FishingState.Failed;
            }
        }
    }

    private void UpdateBobberLocation()
    {
        bobberLocation = new Vector3(
            fishDistance * Mathf.Cos(Mathf.PI / 2 + fishAngle),
            fishDistance * Mathf.Sin(Mathf.PI / 2 + fishAngle),
            0
        );
        if (waterRipples.activeSelf)
        {
            var tween = new LocalPositionTween
            {
                to = bobberLocation,
                duration = 1,
            };
            waterRipples.gameObject.AddTween(tween);
        }
        else
        {
            waterRipples.transform.localPosition = bobberLocation;
        }
        if (waterSplash.activeSelf)
        {
            var tween = new LocalPositionTween
            {
                to = bobberLocation,
                duration = 1,
            };
            waterSplash.gameObject.AddTween(tween);
        } else
        {
            waterSplash.transform.localPosition = bobberLocation;
        }
    }

    private void SetNextBobberLocation()
    {
        fishDistance = UnityEngine.Random.Range(bobberMinDistance, bobberMaxDistance);
        fishAngle = Mathf.Deg2Rad * UnityEngine.Random.Range(-bobberMaxAngle, +bobberMaxAngle);
        UpdateBobberLocation();
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        Debug.Log("start fishing...");
        if (IsFishingAllowed() && fishingState == FishingState.NotFishing)
        {
            castingTimer = 0;
            SetNextBobberLocation();
            // tell player animator to play casting animation
            fishingState = FishingState.Casting;
        }
        else if (fishingState != FishingState.NotFishing)
        {
            fishingState = FishingState.NotFishing;
            waterRipples.SetActive(false);
            waterSplash.SetActive(false);
        }
    }

    private void StartFighting()
    {
        waterRipples.SetActive(false);
        waterSplash.SetActive(true);
        fishingState = FishingState.Fighting;
        fightTimer = 0;
        fightTime = UnityEngine.Random.Range(hookedFish.minFightTime, hookedFish.maxFightTime);
        CatchBar.Instance.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (fishingState)
        {
            case FishingState.NotFishing:
                break;
            case FishingState.Casting:
                // waiting till the casting animation finishes
                if (castingTimer > timeToCast)
                {
                    waitingTime = UnityEngine.Random.Range(minWaitingTime, maxWaitingTime);
                    fishingState = FishingState.Waiting;
                    // waterRipples.transform.localPosition = bobberLocation;
                    // waterSplash.transform.localPosition = bobberLocation;
                    waterRipples.SetActive(true);
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
                    hookedFish = fishes[UnityEngine.Random.Range(0, fishes.Length - 1)];
                    StartFighting();
                }
                hookingTimer += Time.deltaTime;
                break;
            case FishingState.Fighting:
                if (fightTimer > fightTime)
                {
                    fishingState = FishingState.Reelable;
                    waterRipples.SetActive(true);
                    waterSplash.SetActive(false);
                    CatchBar.Instance.gameObject.SetActive(true);
                }
                fightTimer += Time.deltaTime;
                break;
            case FishingState.Reelable:
                break;
            case FishingState.Caught:
                waterRipples.SetActive(false);
                waterSplash.SetActive(false);
                CatchBar.Instance.gameObject.SetActive(false);
                Debug.Log($"You caught a {hookedFish.fishName}");
                CatchDisplay.Instance.SetCaughtFish(hookedFish);
                CatchDisplay.Instance.SetActive();
                fishingState = FishingState.NotFishing;
                break;
            case FishingState.Failed:
                waterRipples.SetActive(false);
                waterSplash.SetActive(false);
                CatchBar.Instance.gameObject.SetActive(false);
                fishingState = FishingState.NotFishing;
                Debug.Log($"You failed to catch a fish");
                break;
        }
    }
}
