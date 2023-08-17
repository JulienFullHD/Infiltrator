using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] private int mainMenuIndex;
    [SerializeField] public static bool isPaused;
    [SerializeField] private GameObject pauseMenuOverlay;

    [Header("Other references")]
    [SerializeField] private GhostRunner ghostRunner;


    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !LevelManager.Instance.isWon)
        {
            ChangePauseStatus(!isPaused);
        }
    }

    public void ChangePauseStatus(bool paused)
    {
        isPaused = paused;

        if (isPaused)
            PauseGame();
        else
            UnPauseGame();
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0.0f;
        pauseMenuOverlay.SetActive(true);
    }

    private void UnPauseGame()
    {
        Time.timeScale = 1.0f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (LevelManager.Instance.isWon)
            return;

        pauseMenuOverlay.SetActive(false);
    }

    public void ResetLevel()
    {
        UnPauseGame();

        ghostRunner.StopRun();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitLevel()
    {
        UnPauseGame();

        ghostRunner.StopRun();
        SceneManager.LoadScene(mainMenuIndex);
    }
}
