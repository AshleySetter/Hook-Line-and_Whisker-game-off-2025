using UnityEngine;

public class ReelSuccessRegionUpgrade : MonoBehaviour, UpgradeI
{
    public int GetCost()
    {
        return Mathf.FloorToInt(GetNextValue() / 4);
    }

    public float GetCurrentValue()
    {
        return CatchBar.Instance.GetReelPercentage();
    }

    public string GetCurrentValueString()
    {
        return $"{CatchBar.Instance.GetReelPercentage()}%";
    }

    public float GetNextValue()
    {
        return CatchBar.Instance.GetReelPercentage() + 5;
    }

    public string GetNextValueString()
    {
        return $"{GetNextValue()}%";
    }

    public void Upgrade()
    {
        CatchBar.Instance.SetReelPercentage(GetNextValue());
    }
}
