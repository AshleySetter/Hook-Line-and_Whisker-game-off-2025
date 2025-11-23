using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int coinsNeededForBills;

    private int day;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        day = 1;
        coinsNeededForBills = 3;
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
        }
        if (DayNightTimer.Instance.GetDayActive())
        {
            // spawn cats according to schedule
        } else
        {
            if (DayNightTimer.Instance.GetDayFinished())
            {
                // tell cats to go home without fish
            }
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
        coinsNeededForBills *= 2;
        // get / generate cat spawn schedule for the day
        // put player outside home with bucket equipped
    }

    public void FinishDay()
    {
        // check if player has enough coins to afford bills / rent
        // if so progress to next day
        // else display end of run screen with exit to main menu button and start another run button
    }
}
