using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSubmission : MonoBehaviour
{
    [SerializeField] TMP_InputField inputName;
    [SerializeField] Button submitButton;

    private void Start()
    {
        submitButton.onClick.AddListener(() =>
        {
            SubmitScore();
        });
        
    }

    public void SubmitScore()
    {
        Leaderboard.Instance.SetLeaderboardEntry(
            inputName.text,
            EndRunScreen.Instance.GetFishCaught(), 
            () => submitButton.gameObject.SetActive(false)
        );
    }
}
