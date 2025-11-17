using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CatchBar : MonoBehaviour
{
    public static CatchBar Instance { get; private set; }

    [SerializeField] private Image CatchBarLeft;
    [SerializeField] private Image CatchBarRight;
    [SerializeField] private Image BoostBarLeft;
    [SerializeField] private Image BoostBarRight;
    [SerializeField] private Image ReelMarker;
    [SerializeField] private float percentageDebug;
    [SerializeField] private float boostPercentageDebug;
    private List<float> frequencies;
    private bool pause;


    private float maxReelMarkerMovement = 404;
    private float reelValue;
    private float reelPercentage; // can be upgraded
    private float reelBoostPercentage; // can be upgraded

    private void Awake()
    {
        Instance = this;
        frequencies = new List<float>();
    }

    private void Start()
    {
        SetReelPercentage(percentageDebug);
        SetReelBoostPercentage(boostPercentageDebug);
        pause = false;
    }

    public void SetReelPercentage(float reelPercentage)
    {
        CatchBarLeft.fillAmount = (100 - reelPercentage) / 100 / 2;
        CatchBarRight.fillAmount = (100 - reelPercentage) / 100 / 2;
        this.reelPercentage = reelPercentage;
    }

    public void SetReelBoostPercentage(float reelBoostPercentage)
    {
        BoostBarLeft.fillAmount = reelBoostPercentage / 100;
        BoostBarRight.fillAmount = reelBoostPercentage / 100;
        this.reelBoostPercentage = reelBoostPercentage;
    }

    public bool GetCatchBarInGreen()
    {
        return Mathf.Abs(reelValue) * 100 < reelPercentage;
    }

    public bool GetCatchBarInBoost()
    {
        return Mathf.Abs(reelValue) * 100 < reelBoostPercentage;
    }

    public void Pause()
    {
        pause = true;
    }

    public void UnPause()
    {
        pause = false;
    }

    public void AddFrequency(float frequency)
    {
        frequencies.Add(frequency);
    }

    public void ResetFrequencies()
    {
        frequencies.Clear();
    }

    private void Update()
    {
        if (!pause)
        {
            float value = 0;
            foreach (var frequency in frequencies)
            {
                value += Mathf.Sin(2 * Mathf.PI * frequency * Time.time);
            }
            value /= frequencies.Count;
            reelValue = value;
            ReelMarker.transform.localPosition = new Vector3(
                value * maxReelMarkerMovement,
                ReelMarker.transform.localPosition.y,
                ReelMarker.transform.localPosition.z);
        }
    }
}
