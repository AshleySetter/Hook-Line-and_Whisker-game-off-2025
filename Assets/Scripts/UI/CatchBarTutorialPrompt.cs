using System;
using TMPro;
using UnityEngine;

public class CatchBarTutorialPrompt : MonoBehaviour
{
    public static CatchBarTutorialPrompt Instance { get; private set; }
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] TextMeshProUGUI tutorialTextBox;
    [SerializeField] GameObject arrows;
    [SerializeField] String tutorialText;
    [SerializeField] bool shouldFreezeTime;
    private bool caughtFish;
    private bool showingNow;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        arrows.SetActive(false);
        caughtFish = false;
    }

    public bool GetShowingNow()
    {
        return showingNow;
    }

    public bool GetCaughtFish()
    {
        return caughtFish;
    }

    public void SetCaughtFish()
    {
        caughtFish = true;
    }

    public void Open()
    {
        tutorialPanel.SetActive(true);
        tutorialTextBox.text = tutorialText;
        arrows.SetActive(true);
        EscapeManager.Instance.SetActiveWindow(tutorialPanel);
        if (shouldFreezeTime) {
            TutorialPrompt.FreezeTime();
        }
        showingNow = true;
    }

    public void Close()
    {
        tutorialPanel.SetActive(false);
        tutorialTextBox.text = "";
        arrows.SetActive(false);
        showingNow = false;
    }

}
