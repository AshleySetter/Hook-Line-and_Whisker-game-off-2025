using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayNightTimer : MonoBehaviour
{
    [SerializeField] private Image dayTimerImage;
    [SerializeField] private Volume GlobalVolume;
    private float minFillAmount = 0.150f;
    private float maxFillAmount = 0.850f;
    private float secondsInADay = 20;
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
            UpdateLighting();
        }
        if (dayTimer > secondsInADay)
        {
            dayTimer = 0;
        }
        dayTimer += Time.deltaTime;
        
    }

    private void UpdateDayTimerVisual()
    {
        float totalFillAmount = (maxFillAmount - minFillAmount);
        float fractionFilled = dayTimer / secondsInADay;
        dayTimerImage.fillAmount = minFillAmount + fractionFilled * totalFillAmount;
    }

    private void UpdateLighting()
    {
        if (GlobalVolume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            float secondsInAnEvening = secondsInADay / 4;
            float secondsDuringDaytime = 3f * secondsInADay / 4f;
            bool isEvening = dayTimer > secondsDuringDaytime;
            float eveningFraction = 0;
            if (isEvening) {
                eveningFraction = (dayTimer - secondsDuringDaytime) / secondsInAnEvening;
            }
            Debug.Log($"{dayTimer} {secondsDuringDaytime} {eveningFraction} {secondsInADay}");
            colorAdjustments.postExposure.value = 0f - 1.5f * eveningFraction;
            colorAdjustments.saturation.value = 0f - 15 * eveningFraction;
            Vector4 rgba = new Vector4(1f, 1f - 0.5f * eveningFraction, 1f - 0.5f * eveningFraction, 1f);
            colorAdjustments.colorFilter.value = new Color(rgba.x, rgba.y, rgba.z, rgba.w);
        }
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
