using UnityEngine;

public class HookUpgrade : MonoBehaviour, UpgradeI
{
    public int GetCost()
    {
        return 2 * PlayerFishing.Instance.GetNumberOfHooks();
    }

    public float GetCurrentValue()
    {
        return PlayerFishing.Instance.GetNumberOfHooks();
    }

    public string GetCurrentValueString()
    {
        return PlayerFishing.Instance.GetNumberOfHooks().ToString();
    }

    public float GetNextValue()
    {
        return PlayerFishing.Instance.GetNumberOfHooks() + 1;
    }

    public string GetNextValueString()
    {
        return (PlayerFishing.Instance.GetNumberOfHooks() + 1).ToString();
    }

    public void Upgrade()
    {
        PlayerFishing.Instance.AddFishingHook();
    }
}
