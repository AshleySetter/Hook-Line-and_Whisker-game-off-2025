using UnityEngine;

[CreateAssetMenu(fileName = "FishSO", menuName = "Scriptable Objects/FishSO")]
public class FishSO : ScriptableObject
{
    public float minFightTime; // min time spent fighting
    public float maxFightTime; // max time spent fighting
    public float yankDistance; // distance yanked by a reel action
    public float fightOnReelChance; // change that it begins fighting again on a reel action
    public float fishWeight; // unit of weight for fish - used to determine how many cats it feeds and value when sold
    public Sprite fishSprite;
}
