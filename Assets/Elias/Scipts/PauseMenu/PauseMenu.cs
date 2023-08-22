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

    /// <summary>
    /// Check for pause button key
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !LevelManager.Instance.isWon)
        {
            ChangePauseStatus(!isPaused);
        }
    }

    /// <summary>
    /// Change state of pause menu
    /// </summary>
    /// <param name="paused">Is paused</param>
    public void ChangePauseStatus(bool paused)
    {
        isPaused = paused;

        if (isPaused)
            PauseGame();
        else
            UnPauseGame();
    }

    /// <summary>
    /// Pauses the game and shows pause menu
    /// </summary>
    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0.0f;
        pauseMenuOverlay.SetActive(true);
    }

    /// <summary>
    /// Unpauses the game and hides pause menu
    /// </summary>
    private void UnPauseGame()
    {
        Time.timeScale = 1.0f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (LevelManager.Instance.isWon)
            return;

        pauseMenuOverlay.SetActive(false);
    }

    /// <summary>
    /// Load the current scene again to reset everything
    /// </summary>
    public void ResetLevel()
    {
        UnPauseGame();

        ghostRunner.StopRun();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Exits the level back to the main menu
    /// </summary>
    public void ExitLevel()
    {
        UnPauseGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ghostRunner.StopRun();
        SceneManager.LoadScene(mainMenuIndex);
    }
}
