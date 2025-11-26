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
        arrow.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance.GetDay() == dayToTrigger) {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                tutorialPanel.SetActive(true);
                tutorialTextBox.text = tutorialText;
                arrow.SetActive(true);
                EscapeManager.Instance.SetActiveWindow(tutorialPanel);
                if (shouldFreezeTime) {
                    FreezeTime();
                }
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
                arrow.SetActive(false);
            }
        }
    }

}
