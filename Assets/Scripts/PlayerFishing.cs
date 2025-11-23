using System;
using System.Collections.Generic;
using Tweens;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFishing : MonoBehaviour
{
    public static PlayerFishing Instance { get; private set; }

    [SerializeField] private FishSO[] fishes;
    [SerializeField] private Tilemap waterTileMap;
    [SerializeField] private TileBase waterTileBase;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip castSound;
    [SerializeField] private GameObject fishingHookPrefab;

    private FishingState fishingState;
    private float timeToCast = 1f; // length of cast animation
    private float castingTimer;
    private int numberOfHooks; // can be upgraded
    private float reelDistance; // can be upgraded
    private List<FishingHook> fishingHooks;
    private int initialNumberOfHooks = 1;


    public enum FishingState
    {
        NotFishing,
        Casting,
        Waiting,
        Finished
    }

    public enum BobberState
    {
        Fighting,
        Waiting,
        Reelable,
        NotVisible
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
        // only allow when next to and facing water
        // and when during the fishing part of the day
        return IsFacingWater() && !DayNightTimer.Instance.GetDayFinished();
    }

    public bool GetIsFishing()
    {
        return fishingState != FishingState.NotFishing;
    }

    private void Awake()
    {
        Instance = this;
        numberOfHooks = 0;
        reelDistance = 0.5f;
        fishingHooks = new List<FishingHook>();
    }

    private void Start()
    {
        for (int i = 0; i < initialNumberOfHooks; i++)
        {
            AddFishingHook();
        }
        fishingState = FishingState.NotFishing;
        animator.SetBool("IsFishing", false);
        castingTimer = 0;
        CatchBar.Instance.gameObject.SetActive(false);
    }

    public void AddFishingHook()
    {
        GameObject fishingHookObj = Instantiate(fishingHookPrefab, transform);
        FishingHook fishingHook = fishingHookObj.GetComponent<FishingHook>();
        fishingHooks.Add(fishingHook);
        numberOfHooks += 1;
    }

    public int GetNumberOfHooks()
    {
        return numberOfHooks;
    }

    public float GetReelDistance()
    {
        return reelDistance;
    }

    public FishSO[] GetFishes()
    {
        return fishes;
    }

    public void StartFishing()
    {
        castingTimer = 0;
        foreach (FishingHook hook in fishingHooks)
        {
            hook.SetNextBobberLocation();
        }
        animator.SetBool("IsFishing", true);
        fishingState = FishingState.Casting;
        SoundFXManager.Instance.PlaySoundFXClip(castSound, transform, 1f, 0.8f, 1.2f);
    }
    
    public void StopFishing()
    {
        animator.SetBool("IsFishing", false);
        fishingState = FishingState.NotFishing;
        foreach (FishingHook hook in fishingHooks)
        {
            hook.StopFishing();
        }
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
                    fishingState = FishingState.Waiting;
                    foreach (FishingHook hook in fishingHooks)
                    {
                        hook.StartFishing();
                    }
                }
                castingTimer += Time.deltaTime;
                break;
            case FishingState.Waiting:
                bool allHooksFinished = true;
                foreach (FishingHook hook in fishingHooks)
                {
                    if (
                        hook.GetHookState() != FishingHook.HookState.Caught &&
                        hook.GetHookState() != FishingHook.HookState.Failed &&
                        hook.GetHookState() != FishingHook.HookState.AfterFailed &&
                        hook.GetHookState() != FishingHook.HookState.NotFishing
                        )
                    {
                        allHooksFinished = false;
                    }
                }
                if (allHooksFinished)
                {
                    fishingState = FishingState.Finished;
                }
                break;
            case FishingState.Finished:
                StopFishing();
                break;
        }
    }
}
