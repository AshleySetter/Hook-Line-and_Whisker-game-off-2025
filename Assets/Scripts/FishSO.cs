using UnityEngine;
using Tweens;

[CreateAssetMenu(fileName = "FishSO", menuName = "Scriptable Objects/FishSO")]
public class FishSO : ScriptableObject
{
    public string fishName;
    public float minFightTime; // min time spent fighting
    public float maxFightTime; // max time spent fighting
    public float reelDistance; // distance reeled by a reel action
    public float fightOnReelChance; // chance that it begins fighting again on a reel action
    public float fishWeight; // unit of weight for fish - used to determine how many cats it feeds and value when sold
    public Sprite fishSprite;
    public float reelTweenDuration;
    public EaseType reelTweenEaseType;
    public float reelPercentageGreen;
}
