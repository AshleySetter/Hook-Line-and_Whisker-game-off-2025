using UnityEngine;

public class ReelBoostRegionUpgrade : MonoBehaviour, UpgradeI
{
    public int GetCost()
    {
        return Mathf.FloorToInt(GetNextValue() / 4);
    }

    public float GetCurrentValue()
    {
        return CatchBar.Instance.GetReelBoostPercentage();
    }

    public string GetCurrentValueString()
    {
        return $"{CatchBar.Instance.GetReelBoostPercentage()}%";
    }

    public float GetNextValue()
    {
        return CatchBar.Instance.GetReelBoostPercentage() + 5;
    }

    public string GetNextValueString()
    {
        return $"{GetNextValue()}%";
    }

    public void Upgrade()
    {
        CatchBar.Instance.SetReelBoostPercentage(GetNextValue());
    }
}
