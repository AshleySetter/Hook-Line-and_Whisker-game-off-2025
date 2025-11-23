using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EndRunScreen : MonoBehaviour
{
    public static EndRunScreen Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI daysSurvivedCounter;
    [SerializeField] private TextMeshProUGUI fishCaughtCounter;
    [SerializeField] private TextMeshProUGUI fishStolenCounter;
    [SerializeField] private TextMeshProUGUI coinsCollectedCounter;
    [SerializeField] private TextMeshProUGUI upgradesBoughtCounter;

    [SerializeField] private UnityEngine.UI.Button playAgainButton;
    [SerializeField] private UnityEngine.UI.Button mainMenuButton;

    private int daysSurvived;
    private int fishCaught;
    private int fishStolen;
    private int coinsCollected;
    private int upgradesBought;

    private void Awake()
    {
        Instance = this;
        daysSurvived = 0;
        fishCaught = 0;
        fishStolen = 0;
        coinsCollected = 0;
        upgradesBought = 0;

        playAgainButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        Time.timeScale = 1f;
    }

    private void Update()
    {
        daysSurvivedCounter.text = daysSurvived.ToString();
        fishCaughtCounter.text = fishCaught.ToString();
        fishStolenCounter.text = fishStolen.ToString();
        coinsCollectedCounter.text = coinsCollected.ToString();
        upgradesBoughtCounter.text = upgradesBought.ToString();
    }

    public void AddToDaysSurvived(int number) {
        daysSurvived += number;
    }

    public void AddToFishCaught(int number) {
        fishCaught += number;
    }

    public void AddToFishStolen(int number) {
        fishStolen += number;
    }

    public void AddToCoinsCollected(int number) {
        coinsCollected += number;
    }

    public void AddToUpgradesBought(int number) {
        upgradesBought += number;
    }
}
