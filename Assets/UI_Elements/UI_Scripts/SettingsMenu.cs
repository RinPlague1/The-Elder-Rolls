using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject GameSettings;
    public GameObject SoundSettings;
    public GameObject DisplaySettings;
    public GameObject Keybindings;
    public GameObject ActiveWindow;

    private void Start()
    {
        ActiveWindow = GameSettings;
    }

    public void GameButtonToggle()
    {
        ActiveWindow.SetActive(false);
        GameSettings.SetActive(true);
        ActiveWindow = GameSettings;
    }

    public void SoundButtonToggle()
    {
        ActiveWindow.SetActive(false);
        SoundSettings.SetActive(true);
        ActiveWindow = SoundSettings;
    }

    public void DisplayButtonToggle()
    {
        ActiveWindow.SetActive(false);
        DisplaySettings.SetActive(true);
        ActiveWindow = DisplaySettings;
    }

    public void KeybindButtonToggle()
    {
        ActiveWindow.SetActive(false);
        Keybindings.SetActive(true);
        ActiveWindow = Keybindings;
    }

    public void ReturnToMainMenu()
    {
        ActiveWindow.SetActive(false);
        GameSettings.SetActive(true);
        ActiveWindow = GameSettings;
        MainMenu.SetActive(true);
        transform.gameObject.SetActive(false);
    }

}
