using System;
using UnityEngine;

public interface UpgradeI
{
    int GetCost();
    float GetMaxValue();
    float GetCurrentValue();
    float GetNextValue();
    string GetCurrentValueString();
    string GetNextValueString();
    void Upgrade();
}
