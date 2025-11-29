using System;
using TMPro;
using UnityEngine;

public class TutorialPrompt : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] TextMeshProUGUI tutorialTextBox;
    [SerializeField] GameObject arrow;
    [SerializeField] String tutorialText;
    [SerializeField] int dayToTrigger;
    [SerializeField] bool shouldFreezeTime;
    [SerializeField] bool shownOnce;

    private bool shownToPlayer;

    public static bool timeFrozen;

    public static void FreezeTime()
    {
        Time.timeScale = 0f;
        UnfreezeTimeInstructionPanel.Instance.gameObject.SetActive(true);
        TutorialPrompt.timeFrozen = true;
    }

    public static void UnfreezeTime()
    {
        Time.timeScale = 1f;
        UnfreezeTimeInstructionPanel.Instance.gameObject.SetActive(false);
        TutorialPrompt.timeFrozen = false;
    }


    private void Start()
    {
        if (arrow != null) {
            arrow.SetActive(false);
        }
        shownToPlayer = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance.GetDay() == dayToTrigger && !DayNightTimer.Instance.GetDayFinished()) {
            if (shownOnce && shownToPlayer) return;
            if (other.TryGetComponent(out PlayerMovement player))
            {
                tutorialPanel.SetActive(true);
                tutorialTextBox.text = tutorialText;
                if (arrow != null) {
                    arrow.SetActive(true);
                }
                EscapeManager.Instance.SetActiveWindow(tutorialPanel);
                if (shouldFreezeTime) {
                    FreezeTime();
                }
                shownToPlayer = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (GameManager.Instance.GetDay() == dayToTrigger) {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                tutorialPanel.SetActive(false);
                tutorialTextBox.text = "";
                if (arrow != null) {
                    arrow.SetActive(false);
                }
            }
        }
    }

}
