using UnityEngine;

public class PromptDisplay : MonoBehaviour
{
    [SerializeField] private GameObject reelPrompt;
    
    private void Start()
    {
        reelPrompt.SetActive(false);
    }

    private void Update()
    {
        if (PlayerFishing.Instance.IsFishingAllowed() && !PlayerFishing.Instance.GetIsFishing())
        {
            reelPrompt.SetActive(false);
        } else if (PlayerFishing.Instance.GetIsFishing())
        {
            reelPrompt.SetActive(true);
        } else
        {
            reelPrompt.SetActive(false);
        }
    }
}
