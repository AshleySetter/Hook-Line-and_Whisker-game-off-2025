using UnityEngine;

public class PromptDisplay : MonoBehaviour
{
    [SerializeField] private GameObject fishingPrompt;
    [SerializeField] private GameObject reelPrompt;
    
    private void Start()
    {
        fishingPrompt.SetActive(false);
        reelPrompt.SetActive(false);
    }

    private void Update()
    {
        if (PlayerFishing.Instance.IsFishingAllowed() && !PlayerFishing.Instance.GetIsFishing())
        {
            fishingPrompt.SetActive(true);
            reelPrompt.SetActive(false);
        } else if (PlayerFishing.Instance.GetIsFishing())
        {
            reelPrompt.SetActive(true);
            fishingPrompt.SetActive(false);
        } else
        {
            fishingPrompt.SetActive(false);
            reelPrompt.SetActive(false);
        }
    }
}
