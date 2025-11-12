using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatchDisplay : MonoBehaviour
{
    public static CatchDisplay Instance { get; private set; }
    private FishSO caughtFish;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject catchDisplay;
    [SerializeField] private Image fishImage;
    [SerializeField] private TextMeshProUGUI caughtFishNameText;

    private void Awake()
    {
        Instance = this;
        closeButton.onClick.AddListener(() =>
        {
            catchDisplay.SetActive(false);
        });
    }

    private void Start()
    {
        caughtFish = null;
        catchDisplay.SetActive(false);
    }

    public void SetCaughtFish(FishSO fishSO)
    {
        caughtFish = fishSO;
        fishImage.sprite = caughtFish.fishSprite;
        caughtFishNameText.text = caughtFish.fishName;
    }

    public void SetActive()
    {
        catchDisplay.SetActive(true);
    }
}
