using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Dan.Main;

public class LevelManager : MonoBehaviour
{
    [Header("Singleton")]
    [SerializeField] public static LevelManager Instance;

    [Header("Level Details")]
    [SerializeField] private int levelIdx;
    [SerializeField] private string levelName;

    [Header("Enemy Details")]
    [ReadOnly, SerializeField] private int enemyCount; // Takes count on level Start()

    [Header("Win Event")]
    [SerializeField] private bool isWon;
    [SerializeField] private CanvasGroup playerCanvasGroup;     // Fade out on win
    [SerializeField] private GameObject winCanvasObject; // Fade  in on win
    [SerializeField] private CanvasGroup winCanvasGroup; // Fade  in on win
    [SerializeField] private float fadeMaxTime;
    [ReadOnly, SerializeField] private float fadeTimer;
    [SerializeField]private ScoreManger scoreManager;
    [SerializeField]private GhostRunner ghostRunner;
    [SerializeField]private string userName;
    [SerializeField]private SetupConfig setupConfig;
    private string publicKey = "e9856340c08efde8a7d87ae37c65bf62b267bd68b4526abeae5c98859244f1c5";

    //Wwise shit
    [Header("Wwise Event")]
    public AK.Wwise.Event PlayMainTheme;
    public AK.Wwise.Event PlayWinTheme;

    private void Start()
    {
        // if(Instance is not null)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        Instance = this;
        isWon = false;
        PlayMainTheme.Post(gameObject); //Wwise MainTheme
        EnemyCount = FindObjectsOfType(typeof(AI_HPSystem)).Count();
    }

    public int EnemyCount
    {
        get
        {
            return enemyCount;
        }
        set
        {
            enemyCount = value;
            if(enemyCount == 0)
            {
                Win();
            }
        }
    }

    public void Win()
    {
        isWon = true;
        winCanvasObject.SetActive(true);
        PlayWinTheme.Post(gameObject);
        fadeTimer = 0;
        ghostRunner.StopRun();

        Debug.Log(scoreManager.GetScore());
        if(ghostRunner._system.GetRun(RecordingType.Last, out Recording run))
        {
            //ghostRunner._system.SetSavedRun(run);
            //ghostRunner._system.SetSavedRun(run);
            setupConfig.SaveRun(run.Serialize());
            Debug.Log(run.Serialize());
        }
        //if(ghostRunner._system.GetRun(RecordingType.Last, out Recording run)) leaderBoard.SetLeaderBoardEntry(userName, scoreManager.GetScore(), "NULL");//run.Serialize()
        //Debug.Log(run.Serialize().ToString().Length);
        userName = PlayerPrefs.GetString("PlayerName");
        //SetLeaderBoardEntry(userName, scoreManager.GetScore());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    private void Update()
    {
        if (isWon)
        {
            fadeTimer += Time.deltaTime;

            playerCanvasGroup.alpha = 1 - (fadeTimer / fadeMaxTime);
            winCanvasGroup.alpha = fadeTimer / fadeMaxTime;


        }
    }

    public void SetLeaderBoardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicKey, username, score, ((msg) => {
        }));
    }
}
