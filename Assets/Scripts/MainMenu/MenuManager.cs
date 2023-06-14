
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Menu[] menuArray;

    private void Start()
    {
        OpenMenu(menuArray[0]);
    }

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

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
