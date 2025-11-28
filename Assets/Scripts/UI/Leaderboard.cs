using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Dan.Main;
using System.Linq;
using System;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance {get; private set; }
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;
    private bool scoreSubmitted;

    private string publicLeaderboardKey = "11ba5d264acbcb80117ce3b584a0718621015c272185a71e74a47412ca612d63";

    private void Awake()
    {
        Instance = this;
        scoreSubmitted = false;
    }

    private void Start()
    {
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (msg =>
        {
            int count = 0;
            if (msg.Length <= names.Count)
            {
                count = msg.Length;
            }
            if (names.Count <= msg.Length)
            {
                count = names.Count;
            }
            for (int i = 0; i < count; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score, Action successCallback)
    {
        if (!scoreSubmitted) {
            LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((success) =>
            {
                if (!success)
                {
                    Debug.Log("Uploading score failed");
                } else
                {
                    GetLeaderboard();
                    scoreSubmitted = true;
                    successCallback();
                }
            }));
        }
    } 
}
