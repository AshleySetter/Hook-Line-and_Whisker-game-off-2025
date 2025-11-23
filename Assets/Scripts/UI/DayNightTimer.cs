using UnityEngine;
using UnityEngine.UI;

public class DayNightTimer : MonoBehaviour
{
    [SerializeField] private Image dayTimerImage;
    private float minFillAmount = 0.150f;
    private float maxFillAmount = 0.850f;
    private float secondsInADay = 60;
    private bool dayTimerStarted;
    private float dayTimer;

    private void Start()
    {
        dayTimerStarted = false;
        StartDayTimer();
    }

    void Update()
    {
        if (dayTimerStarted && dayTimer <= secondsInADay)
        {
            UpdateDayTimerVisual();
        }
        dayTimer += Time.deltaTime;
    }

    private void UpdateDayTimerVisual()
    {
        float totalFillAmount = (maxFillAmount - minFillAmount);
        float fractionFilled = dayTimer / secondsInADay;
        dayTimerImage.fillAmount = minFillAmount + fractionFilled * totalFillAmount;
    }

    public void ResetDayTimer()
    {
        dayTimer = 0;
    }

    public void StartDayTimer()
    {
        dayTimer = 0;
        dayTimerStarted = true;
    }

    public bool GetDayTimerStarted()
    {
        return dayTimerStarted;
    }
}
