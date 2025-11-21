using System;
using TMPro;
using UnityEngine;

public class FishBucketCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fishCounter;

    private void Start()
    {
        FishBucket.Instance.OnFishCountChanged += RefreshUI;
        RefreshUI();
    }

    private void RefreshUI()
    {
        String fishNumber = $"{FishBucket.Instance.GetNumberOfFish()}";
        if (fishNumber != fishCounter.text) {
            fishCounter.text = fishNumber;
        }
    }
}
