using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Btn_Functions : MonoBehaviour
{
    public Canvas Main;
    public Canvas Settings;
    public Canvas charCreator;

    public void Open_Settings()
    {
        Main.enabled = false;
        Settings.enabled = true;
        charCreator.enabled = false;
    }

    public void CharacterController()
    {
        Main.enabled = false;
        Settings.enabled = false;
        charCreator.enabled = true;

    }

    public void returnToMenu()
    {
        Main.enabled = true;
        Settings.enabled = false;
        charCreator.enabled = false;
    }


    public void LoadGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
