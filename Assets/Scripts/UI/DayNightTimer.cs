using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayNightTimer : MonoBehaviour
{
    public static DayNightTimer Instance {get; private set; }
    [SerializeField] private Image dayTimerImage;
    [SerializeField] private Volume GlobalVolume;
    private float minFillAmount = 0.150f;
    private float maxFillAmount = 0.850f;
    private float secondsInADay = 10;
    private bool dayStarted;
    private float dayTimer;
    private bool dayFinished;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dayStarted = false;
        dayFinished = false;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        UpdateDayTimerVisual();
        UpdateLighting();
    }

    private void Update()
    {
        if (GetDayActive())
        {
            UpdateDayTimerVisual();
            UpdateLighting();
            dayTimer += Time.deltaTime;
        }
        if (dayTimer > secondsInADay)
        {
            dayFinished = true;
        }
    }

    public bool GetDayActive()
    {
        return dayStarted && !dayFinished;
    }

    public bool GetDayFinished()
    {
        return dayFinished;
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
            colorAdjustments.postExposure.value = 0f - 1.5f * eveningFraction;
            colorAdjustments.saturation.value = 0f - 15 * eveningFraction;
            Vector4 rgba = new Vector4(1f, 1f - 0.5f * eveningFraction, 1f - 0.5f * eveningFraction, 1f);
            colorAdjustments.colorFilter.value = new Color(rgba.x, rgba.y, rgba.z, rgba.w);
        }
    }

    public void ResetDayTimer()
    {
        dayTimer = 0;
        dayStarted = false;
        dayFinished = false;
    }

    public void StartDay()
    {
        dayStarted = true;
    }

    public void StopDayTimer()
    {
        dayStarted = false;
    }

    public bool GetDayTimerStarted()
    {
        return dayStarted;
    }
}
