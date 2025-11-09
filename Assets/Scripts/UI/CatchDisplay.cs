using UnityEngine;
using UnityEngine.UI;

public class CatchDisplay : MonoBehaviour
{
    public static CatchDisplay Instance { get; private set; }
    private FishSO caughtFish;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject catchDisplay;

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
    }

    public void SetActive()
    {
        catchDisplay.SetActive(true);
    }
}
