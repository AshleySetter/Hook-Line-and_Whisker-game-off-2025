using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int coinsNeededForBills;

    private int day;

    private void Awake()
    {
        Instance = this;
        day = 0;
    }

    private void Start()
    {
        ProgressToNextDay();
        coinsNeededForBills = 2;
    }

    private void Update()
    {
        if (
            PlayerFishing.Instance.GetIsFishing() &&
            !DayNightTimer.Instance.GetDayActive() && 
            !DayNightTimer.Instance.GetDayFinished()
            ) 
        {
           DayNightTimer.Instance.StartDay();
           CatSpawnScheduler.Instance.StartSpawning();
        }
    }

    public int GetDay()
    {
        return day;
    }

    public int GetBills()
    {
        return coinsNeededForBills;
    }

    public void ProgressToNextDay()
    {
        day += 1;
        DayNightTimer.Instance.StopDayTimer();
        DayNightTimer.Instance.ResetDayTimer();
        DayNightTimer.Instance.UpdateVisuals();
        coinsNeededForBills *= 2;

        // get / generate cat spawn schedule for the day
        CatSpawnScheduler.Instance.GenerateSpawnSchedule(day * day / 2 - 1); 

        // put player outside home with bucket equipped
        if (!FishBucket.Instance.IsHeldByPlayer()) {
            FishBucket.Instance.PickUp();
        }
        PlayerMovement.Instance.MovePlayerToFrontDoor();
    }

    public void FinishDay()
    {
        if (FishMarket.Instance.GetCoins() >= coinsNeededForBills)
        {
            FishMarket.Instance.RemoveCoins(coinsNeededForBills);
            ProgressToNextDay();
        } else
        {
            // display end of run screen with exit to main menu button and start another run button   
        }
    }
}
