using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Dan.Main;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Singleton")]
    [SerializeField] public static LevelManager Instance;

    [Header("Level Details")]
    [SerializeField] private int levelIdx; //Unused
    [SerializeField] private string levelName; //Unused

    [Header("Enemy Details")]
    [ReadOnly, SerializeField] private int enemyCount; // Takes count on level Start()

    [Header("Win Event")]
    [SerializeField] public bool isWon;
    [SerializeField] private int mainMenuIndex;
    [SerializeField] private CanvasGroup playerCanvasGroup;
    [SerializeField] private GameObject winCanvasObject;
    [SerializeField] private float fadeMaxTime;
    [ReadOnly, SerializeField] private float fadeTimer;
    [SerializeField]private ScoreManger scoreManager;
    [SerializeField] private TextMeshProUGUI winScreenScoreText;
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
                StartCoroutine(WinInSeconds(0.2f));
            }
        }
    }

    /// <summary>
    /// Delay the winning sequence to let other code finish first
    /// </summary>
    /// <param name="seconds">Delay duration in seconds</param>
    /// <returns></returns>
    private IEnumerator WinInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Win();
    }

    /// <summary>
    /// Winning sequence
    /// </summary>
    public void Win()
    {
        Time.timeScale = 0;

        isWon = true;
        PauseMenu.isPaused = true;
        winCanvasObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayWinTheme.Post(gameObject);
        ghostRunner.StopRun();

        playerCanvasGroup.alpha = 0.5f;
        scoreManager.ForceStopCombo();
        winScreenScoreText.text = scoreManager.GetScore().ToString();

        if (ghostRunner._system.GetRun(RecordingType.Last, out Recording run))
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
    }

    public void SetLeaderBoardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicKey, username, score, ((msg) => {
        }));
    }

    /// <summary>
    /// Load the current scene again to reset everything
    /// </summary>
    public void ResetLevel()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Exits the level back to the main menu
    /// </summary>
    public void ExitLevel()
    {
        Time.timeScale = 1.0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(mainMenuIndex);
    }
}
