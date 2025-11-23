using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] MonoBehaviour upgradeBehaviour;
    UpgradeI upgrade;
    [SerializeField] Button upgradeButton;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] TextMeshProUGUI currentValue;
    [SerializeField] TextMeshProUGUI nextValue;

    private void Awake()
    {
        upgrade = upgradeBehaviour as UpgradeI;
        if (upgrade == null)
        {
            Debug.LogError($"{name}: Assigned component does not implement UpgradeI!");
        }
    }

    private void Start()
    {
        RefreshUI();
        upgradeButton.onClick.AddListener(() =>
        {
            if (
                upgrade.GetCost() <= (FishMarket.Instance.GetCoins() - GameManager.Instance.GetBills()) &&
                upgrade.GetCurrentValue() < upgrade.GetMaxValue()
            ) {
                // play sound of money spent
                FishMarket.Instance.RemoveCoins(upgrade.GetCost());
                upgrade.Upgrade();
                EndRunScreen.Instance.AddToUpgradesBought(1);
                RefreshUI();
            } else
            {
                // play not enough money sound
            }
        });
    }

    private void RefreshUI()
    {
        cost.text = upgrade.GetCost().ToString();
        currentValue.text = upgrade.GetCurrentValueString();
        if (upgrade.GetCurrentValue() < upgrade.GetMaxValue())
        {
            nextValue.text = upgrade.GetNextValueString();
        } else {
            nextValue.text = upgrade.GetCurrentValueString();
        }
    }
}
