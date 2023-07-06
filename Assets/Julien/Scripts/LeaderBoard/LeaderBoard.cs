using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField]private List<TextMeshProUGUI> names;
    [SerializeField]private List<TextMeshProUGUI> scores;
    [SerializeField]private ReplaySystem replaySystem;
    [SerializeField]private GhostRunner ghostRunner;
    private string publicKey = "e9856340c08efde8a7d87ae37c65bf62b267bd68b4526abeae5c98859244f1c5";

    private void Start() 
    {
        GetLeaderBoard();
        ghostRunner.StartReplay();

    }

    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicKey, ((msg) => {
            int loopLenght = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLenght; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
                //Recording run = new Recording(msg[0].Extra);
                //replaySystem.SetSavedRun(run);
            }
        }));
    }

    public void SetLeaderBoardEntry(string username, int score, string ghostRunnerData)
    {
        LeaderboardCreator.UploadNewEntry(publicKey, username, score, ((msg) => {
            GetLeaderBoard();
        }));
    }
}
