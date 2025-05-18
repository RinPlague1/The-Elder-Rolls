using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuDisplay : MonoBehaviour
{
    private bool MenuEnabled = false;
    public bool IsDay = true;
    public GameObject MainMenu;
    public GameObject NightDisplay;
    public GameObject DayDisplay;
    public TextMeshProUGUI WeatherText;

    private void Start()
    {
        ToggleMenu();
        ChangeTime(IsDay);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            MenuEnabled = !MenuEnabled;
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        bool updateMenu = (MenuEnabled == true) ? true : false;
        MainMenu.SetActive(updateMenu);
    }

    void ChangeTime(bool ChangeToDay)
    {
        IsDay = ChangeToDay;
        if (IsDay)
        {
            NightDisplay.SetActive(false);
            DayDisplay.SetActive(true);
            WeatherText.text = "DAY";
        }
        else
        {
            NightDisplay.SetActive(true);
            DayDisplay.SetActive(false);
            WeatherText.text = "NIGHT";
        }
    }
}
