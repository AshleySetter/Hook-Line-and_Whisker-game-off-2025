using System;
using TMPro;
using UnityEngine;

public class BillsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCounter;

private void Start()
    {
        FishMarket.Instance.OnCoinsChanged += RefreshUI;
        RefreshUI();
    }

    private void Update()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        String billsNumber = $"{GameManager.Instance.GetBills()}";
        if (billsNumber != coinCounter.text) {
            coinCounter.text = billsNumber;
        }
    }
}
