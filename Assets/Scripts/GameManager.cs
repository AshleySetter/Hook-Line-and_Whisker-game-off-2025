using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject endRunScreen;
    private int coinsNeededForBills;

    private int day;

    private void Awake()
    {
        Instance = this;
        day = 0;
        // endRunScreen.SetActive(false);
    }

    private void Start()
    {
        ProgressToNextDay();
        coinsNeededForBills = 1;
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
        coinsNeededForBills += 1;

        // get / generate cat spawn schedule for the day
        CatSpawnScheduler.Instance.GenerateSpawnSchedule(day * day / 2 - 1);

        // put player outside home with bucket equipped
        if (!FishBucket.Instance.IsHeldByPlayer()) {
            FishBucket.Instance.PickUp();
        }
        PlayerMovement.Instance.MovePlayerToFrontDoor();

        Time.timeScale = 1f;
    }

    public void FinishDay()
    {
        if (FishMarket.Instance.GetCoins() >= coinsNeededForBills)
        {
            FishMarket.Instance.RemoveCoins(coinsNeededForBills);
            ProgressToNextDay();
            EndRunScreen.Instance.AddToDaysSurvived(1);
        } else
        {
            // pause time and display end of run screen
            Time.timeScale = 0f;
            endRunScreen.SetActive(true);
        }
    }
}
