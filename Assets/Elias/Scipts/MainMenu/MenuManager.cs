
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //Wwise
    [Header("Wwise Events")]
    public AK.Wwise.Event PlayMenuSoundtrack;
    public AK.Wwise.Event StopMenuSoundtrack;
    [SerializeField] Menu[] menuArray; 

    private void Start()
    {
        PlayMenuSoundtrack.Post(gameObject);        //Wwise
        OpenMenu(menuArray[0]);
    }

    /// <summary>
    /// Shows a new menu and hides all other shown menues
    /// </summary>
    /// <param name="menu">Menu script</param>
    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menuArray.Length; i++)
        {
            if (menuArray[i].isOpen)
            {
                CloseMenu(menuArray[i]);
            }
        }
        menu.Open();
    }

    /// <summary>
    /// Hides the menu
    /// </summary>
    /// <param name="menu">Menu script</param>
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }


    /// <summary>
    /// Exits out the Application
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Loads the scene via its name as a string
    /// </summary>
    /// <param name="sceneName">Scene name</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
