using System;
using Tweens;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishingHook : MonoBehaviour
{
    [SerializeField] private GameObject waterRipplesPrefab;
    [SerializeField] private GameObject waterSplashPrefab;
    [SerializeField] private GameObject fishShadowPrefab;
    private FishSO[] fishes;
    [SerializeField] private GameObject fishingLinePrefab;
    private GameObject playerVisual;

    private HookState hookState;
    private float timeToHook = 0.5f; // length of hooking fish animation
    private float hookingTimer;
    private float minWaitingTime = 1.5f;
    private float maxWaitingTime = 5f;
    private float waitingTime;
    private float waitingTimer;
    private float fightTime;
    private float fightTimer;
    private Vector3 bobberLocation;
    private float bobberMaxAngle = 20;
    private float bobberMinDistance = 2;
    private float bobberMaxDistance = 8;
    private FishSO hookedFish;
    private float fishAngle;
    private float fishDistance;
    private float catchDistance = 1f;
    private BobberState bobberState;
    private float reelDistance;
    private GameObject waterRipples;
    private GameObject waterSplash;
    private GameObject fishShadow;
    private GameObject fishingLine;

    public enum HookState
    {
        NotFishing,
        Waiting,
        HookedFish,
        Fighting,
        Reelable,
        Caught,
        Failed,
        AfterFailed,
    }

    public enum BobberState
    {
        Fighting,
        Waiting,
        Reelable,
        NotVisible
    }

    private void Start()
    {
        waterRipples = Instantiate(waterRipplesPrefab, PlayerMovement.Instance.transform);
        waterSplash = Instantiate(waterSplashPrefab, PlayerMovement.Instance.transform);
        fishShadow = Instantiate(fishShadowPrefab, PlayerMovement.Instance.transform);
        fishingLine = Instantiate(fishingLinePrefab, PlayerMovement.Instance.GetVisual().transform);
        FishingLine fishingLineEntity = fishingLine.GetComponent<FishingLine>();
        StartCoroutine(DoAfterDelayUtility.DoOnNextFrame(() =>
        {
            fishingLineEntity.SetFishPositionTransforms(new Transform[] {
                waterRipples.transform,
                waterSplash.transform,
                fishShadow.transform
            });
        }));
        fishes = PlayerFishing.Instance.GetFishes();
        GameInput.Instance.OnReelAction += GameInput_OnReelAction;
        hookState = HookState.NotFishing;
        SetNextBobberLocation();
        SetBobberState(BobberState.NotVisible);
        reelDistance = PlayerFishing.Instance.GetReelDistance();
    }

    public void StartFishing()
    {
        waitingTime = UnityEngine.Random.Range(minWaitingTime, maxWaitingTime);
        hookState = HookState.Waiting;
    }
    
    public void StopFishing()
    {
        hookState = HookState.NotFishing;
    }

    private void SetBobberState(BobberState bobberState)
    {
        this.bobberState = bobberState;
        switch (bobberState)
        {
            case BobberState.NotVisible:
                waterRipples.SetActive(false);
                waterSplash.SetActive(false);
                fishShadow.SetActive(false);
                fishingLine.SetActive(false);
                break;
            case BobberState.Fighting:
                waterRipples.SetActive(false);
                waterSplash.SetActive(true);
                fishShadow.SetActive(false);
                fishingLine.SetActive(true);
                break;
            case BobberState.Reelable:
                waterRipples.SetActive(false);
                waterSplash.SetActive(false);
                fishShadow.SetActive(true);
                fishingLine.SetActive(true);
                break;
            case BobberState.Waiting:
                waterRipples.SetActive(true);
                waterSplash.SetActive(false);
                fishShadow.SetActive(false);
                fishingLine.SetActive(true);
                break;
        }

    }

    private void GameInput_OnReelAction(object sender, EventArgs e)
    {
        if (hookState == HookState.Reelable)
        {
            if (CatchBar.Instance.GetCatchBarInGreen())
            {
                if (CatchBar.Instance.GetCatchBarInBoost())
                {
                    Debug.Log("Fish Boost reeled!");
                    fishDistance -= reelDistance * 2;
                } else
                {
                    Debug.Log($"Fish reeled");
                    fishDistance -= reelDistance;
                }
                fishDistance -= reelDistance;
                UpdateBobberLocation();
                if (UnityEngine.Random.Range(0, 100) < hookedFish.fightOnReelChance)
                {
                    StartCoroutine(DoAfterDelayUtility.DoAfterDelay(1f, () =>
                    {
                        if (hookState == HookState.Reelable)
                        {
                            StartFighting();
                        }
                    }));
                }
                if (fishDistance < catchDistance)
                {
                    StartCoroutine(DoAfterDelayUtility.DoAfterDelay(0.5f, () =>
                    {
                        if (hookState == HookState.Reelable)
                        {
                            hookState = HookState.Caught;
                        }
                    }));
                }
            } else
            {
                hookState = HookState.Failed;
            }
        }
        else
        {
            if (hookState != HookState.Caught || hookState != HookState.NotFishing)
            {
                Debug.Log("You reeled at the wrong time!");
                hookState = HookState.Failed;
            }
        }
    }

    private void UpdateBobberLocation()
    {
        Vector3 facingVector = PlayerMovement.Instance.GetFacingVector();
        Vector3 offset = fishDistance * facingVector;
        bobberLocation = Quaternion.Euler(0f, 0f, fishAngle) * offset;
        GameObject[] bobberSprites = new GameObject[] { waterRipples, waterSplash, fishShadow };
        foreach (var sprite in bobberSprites)
        {
            if (sprite.activeSelf)
            {
                var tween = new LocalPositionTween
                {
                    to = bobberLocation,
                    duration = 1,
                };
                sprite.gameObject.AddTween(tween);
            }
            else
            {
                sprite.transform.localPosition = bobberLocation;
            }
        }
    }

    public void SetNextBobberLocation()
    {
        fishDistance = UnityEngine.Random.Range(bobberMinDistance, bobberMaxDistance);
        fishAngle = UnityEngine.Random.Range(-bobberMaxAngle, +bobberMaxAngle);
        UpdateBobberLocation();
    }

    private void StartFighting()
    {
        SetBobberState(BobberState.Fighting);
        hookState = HookState.Fighting;
        fightTimer = 0;
        fightTime = UnityEngine.Random.Range(hookedFish.minFightTime, hookedFish.maxFightTime);
    }

    public HookState GetHookState()
    {
        return hookState;
    }

    private void Update()
    {
        switch (hookState)
        {
            case HookState.NotFishing:
                break;
            case HookState.Waiting:
                if (waitingTimer > waitingTime)
                {
                    hookingTimer = 0;
                    hookState = HookState.HookedFish;
                }
                waitingTimer += Time.deltaTime;
                break;
            case HookState.HookedFish:
                if (hookingTimer > timeToHook)
                {
                    hookedFish = fishes[UnityEngine.Random.Range(0, fishes.Length)];
                    Debug.Log($"hooked fish: {hookedFish}");
                    CatchBar.Instance.AddFrequency(hookedFish.reelFrequency);
                    StartFighting();
                    CatchBar.Instance.gameObject.SetActive(true);
                }
                hookingTimer += Time.deltaTime;
                break;
            case HookState.Fighting:
                if (fightTimer > fightTime)
                {
                    hookState = HookState.Reelable;
                    SetBobberState(BobberState.Reelable);
                }
                fightTimer += Time.deltaTime;
                break;
            case HookState.Reelable:
                break;
            case HookState.Caught:
                SetBobberState(BobberState.NotVisible);
                Debug.Log($"You caught a {hookedFish.fishName}");
                // CatchDisplay.Instance.SetCaughtFish(hookedFish);
                // CatchDisplay.Instance.SetActive();
                Inventory.Instance.AddFish(hookedFish);
                hookState = HookState.NotFishing;
                CatchBar.Instance.RemoveFrequency(hookedFish.reelFrequency);
                break;
            case HookState.Failed:
                SetBobberState(BobberState.NotVisible);
                hookState = HookState.NotFishing;
                Debug.Log($"You failed to catch a fish");
                Debug.Log($"{CatchBar.Instance}, {hookedFish}");
                if (hookedFish != null)
                {
                    CatchBar.Instance.RemoveFrequency(hookedFish.reelFrequency);
                }
                hookedFish = null;
                hookState = HookState.AfterFailed;
                break;
            case HookState.AfterFailed:
                break;
        }
    }
}
