using System;
using Tweens;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFishing : MonoBehaviour
{
    public static PlayerFishing Instance { get; private set; }

    [SerializeField] private GameObject waterRipples;
    [SerializeField] private GameObject waterSplash;
    [SerializeField] private GameObject fishShadow;
    [SerializeField] private FishSO[] fishes;
    [SerializeField] private Tilemap waterTileMap;
    [SerializeField] private TileBase waterTileBase;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip castSound;
    [SerializeField] private GameObject fishingLine;

    private FishingState fishingState;
    private float timeToCast = 1f; // length of cast animation
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
    private float reelDistance => hookedFish.reelDistance;
    private float catchDistance = 1f;
    private BobberState bobberState;
    private float numberOfHooks;

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

    public enum BobberState
    {
        Fighting,
        Waiting,
        Reelable,
        NotVisible
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

    public bool IsFacingWater()
    {
        Vector3 inFrontOfPlayer = PlayerMovement.Instance.GetPositionInFrontOfPlayer();
        Vector3Int tileMapCell = waterTileMap.WorldToCell(inFrontOfPlayer);
        TileBase tileInFrontOfPlayer = waterTileMap.GetTile(tileMapCell);
        return tileInFrontOfPlayer == waterTileBase;
    }

    public bool IsFishingAllowed()
    {
        return IsFacingWater();
        // only allow when next to and facing water
        // and when during the fishing part of the day
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
        GameInput.Instance.OnReelAction += GameInput_OnReelAction;
        fishingState = FishingState.NotFishing;
        animator.SetBool("IsFishing", false);
        castingTimer = 0;
        SetNextBobberLocation();
        SetBobberState(BobberState.NotVisible);
        CatchBar.Instance.gameObject.SetActive(false);
        numberOfHooks = 1;
    }

    private void SetReelGreenPercentage()
    {
        
    }

    private void GameInput_OnReelAction(object sender, EventArgs e)
    {
        if (fishingState != FishingState.NotFishing)
        {
            animator.SetTrigger("DidReel");
        }
        if (fishingState == FishingState.Reelable)
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
                        if (fishingState == FishingState.Reelable)
                        {
                            StartFighting();
                        }
                    }));
                }
                if (fishDistance < catchDistance)
                {
                    StartCoroutine(DoAfterDelayUtility.DoAfterDelay(0.5f, () =>
                    {
                        if (fishingState == FishingState.Reelable)
                        {
                            fishingState = FishingState.Caught;
                        }
                    }));
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

    private void SetNextBobberLocation()
    {
        fishDistance = UnityEngine.Random.Range(bobberMinDistance, bobberMaxDistance);
        fishAngle = UnityEngine.Random.Range(-bobberMaxAngle, +bobberMaxAngle);
        UpdateBobberLocation();
    }

    public void StartFishing()
    {
        castingTimer = 0;
        SetNextBobberLocation();
        animator.SetBool("IsFishing", true);
        fishingState = FishingState.Casting;
        SoundFXManager.Instance.PlaySoundFXClip(castSound, transform, 1f, 0.8f, 1.2f);
    }
    
    public void StopFishing()
    {
        animator.SetBool("IsFishing", false);
        fishingState = FishingState.NotFishing;
        SetBobberState(BobberState.NotVisible);
        CatchBar.Instance.gameObject.SetActive(false);
    }

    private void StartFighting()
    {
        SetBobberState(BobberState.Fighting);
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
                    SetBobberState(BobberState.Waiting);
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
                    hookedFish = fishes[UnityEngine.Random.Range(0, fishes.Length)];
                    Debug.Log(hookedFish);
                    CatchBar.Instance.AddFrequency(hookedFish.reelFrequency);
                    StartFighting();
                }
                hookingTimer += Time.deltaTime;
                break;
            case FishingState.Fighting:
                if (fightTimer > fightTime)
                {
                    fishingState = FishingState.Reelable;
                    SetBobberState(BobberState.Reelable);
                    CatchBar.Instance.gameObject.SetActive(true);
                }
                fightTimer += Time.deltaTime;
                break;
            case FishingState.Reelable:
                break;
            case FishingState.Caught:
                SetBobberState(BobberState.NotVisible);
                CatchBar.Instance.gameObject.SetActive(false);
                Debug.Log($"You caught a {hookedFish.fishName}");
                // CatchDisplay.Instance.SetCaughtFish(hookedFish);
                // CatchDisplay.Instance.SetActive();
                Inventory.Instance.AddFish(hookedFish);
                fishingState = FishingState.NotFishing;
                animator.SetBool("IsFishing", false);
                CatchBar.Instance.ResetFrequencies();
                break;
            case FishingState.Failed:
                SetBobberState(BobberState.NotVisible);
                CatchBar.Instance.gameObject.SetActive(false);
                fishingState = FishingState.NotFishing;
                animator.SetBool("IsFishing", false);
                Debug.Log($"You failed to catch a fish");
                CatchBar.Instance.ResetFrequencies();
                break;
        }
    }
}
