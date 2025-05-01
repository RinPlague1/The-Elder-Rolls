using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Btn_Functions : MonoBehaviour
{
    public Canvas Main;
    public Canvas Settings;
    void Open_Settings()
    {
        Main.enabled = false;
        Settings.enabled = true;
    }


    public void StartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
