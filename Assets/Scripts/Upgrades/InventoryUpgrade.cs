using UnityEngine;

public class InventoryUpgrade : MonoBehaviour, UpgradeI
{
    public int GetCost()
    {
        return 2 * Inventory.Instance.GetCapacity();
    }

    public float GetCurrentValue()
    {
        return Inventory.Instance.GetCapacity();
    }

    public string GetCurrentValueString()
    {
        return Inventory.Instance.GetCapacity().ToString();
    }

    public float GetNextValue()
    {
        return Inventory.Instance.GetCapacity() + 1;
    }

    public string GetNextValueString()
    {
        return (Inventory.Instance.GetCapacity() + 1).ToString();
    }

    public void Upgrade()
    {
        Inventory.Instance.AddCapacity();
    }
}
