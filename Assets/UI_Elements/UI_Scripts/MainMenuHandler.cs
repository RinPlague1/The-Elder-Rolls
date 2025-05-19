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
        OpenQuestTab();
    }

    public void OpenQuestTab()
    {
        ActiveWindow.SetActive(false);
        QuestMenu.SetActive(true);
        ActiveWindow = QuestMenu;
    }

    public void OpenEquipmentTab()
    {
        ActiveWindow.SetActive(false);
        EquipmentMenu.SetActive(true);
        ActiveWindow = EquipmentMenu;
    }

    public void OpenInventoryTab()
    {
        ActiveWindow.SetActive(false);
        InventoryMenu.SetActive(true);
        ActiveWindow = InventoryMenu;
    }

    public void OpenStatusTab()
    {
        ActiveWindow.SetActive(false);
        StatusMenu.SetActive(true);
        ActiveWindow = StatusMenu;
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
