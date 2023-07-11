using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField]private Transform uiParent;
    [SerializeField]private GameObject uiPrefab;
    private string publicKey = "e9856340c08efde8a7d87ae37c65bf62b267bd68b4526abeae5c98859244f1c5";

    private void Start() 
    {
        GetLeaderBoard();
    }

    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicKey, ((msg) => {
            int loopLenght = msg.Length;
            for (int i = 0; i < loopLenght; i++)
            {
                GameObject _uiPrefab = Instantiate(uiPrefab, Vector3.zero, Quaternion.identity, uiParent);
                Debug.Log(_uiPrefab.name);
                Debug.Log(_uiPrefab.GetComponent<ScoreboardUIPrefab>().PlayerName.text);
                _uiPrefab.GetComponent<ScoreboardUIPrefab>().PlayerName.text = msg[i].Username;
                _uiPrefab.GetComponent<ScoreboardUIPrefab>().ScoreText.text = msg[i].Score.ToString();
                //names[i].text = msg[i].Username;
                //scores[i].text = msg[i].Score.ToString();
            }
        }));
    }
}
