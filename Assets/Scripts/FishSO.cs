using UnityEngine;
using Tweens;

[CreateAssetMenu(fileName = "FishSO", menuName = "Scriptable Objects/FishSO")]
public class FishSO : ScriptableObject
{
    public string fishName;
    public float minFightTime; // min time spent fighting
    public float maxFightTime; // max time spent fighting
    public float fightOnReelChance; // chance that it begins fighting again on a reel action
    public float fishWeight; // unit of weight for fish - used to determine how many cats it feeds and value when sold
    public Sprite fishSprite;
    public float reelFrequency; // frequency at which the reel marker moves when trying to reel in this fish
    public int fishValue; // value in coins
}
