using System;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCounter;
    [SerializeField] private Animator coinAnimator;

    private void Start()
    {
        FishMarket.Instance.OnCoinsChanged += RefreshUI;
        RefreshUI();
    }

    private void RefreshUI()
    {
        String coinNumber = $"{FishMarket.Instance.GetCoins()}";
        if (coinNumber != coinCounter.text) {
            coinCounter.text = coinNumber;
            coinAnimator.SetTrigger("Spin");
        }
    }
}
