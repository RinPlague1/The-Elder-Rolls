using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject QuestMenu;
    public GameObject InventoryMenu;
    public GameObject EquipmentMenu;
    public GameObject StatusMenu;
    public GameObject SettingsMenu;
    public GameObject ActiveWindow;

    private void Start()
    {
        ActiveWindow = QuestMenu;
    }

    public void OpenSettingsMenu()
    {
        ActiveWindow.SetActive(false);
        transform.gameObject.SetActive(false);
        SettingsMenu.SetActive(true);
        ActiveWindow = SettingsMenu;
    }

    public void QuitGame()
    {
        Debug.Log("Exit Game");
    }

}
