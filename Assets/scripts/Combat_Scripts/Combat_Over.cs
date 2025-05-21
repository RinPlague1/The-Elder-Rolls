using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Combat_Over : MonoBehaviour
{
    public static Combat_Over Instance { get; private set; }

    [Header("UI References")]
    public GameObject Combat_End_Popup;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Level_Up_Description;
    public Button Exit_Button;
    
    public Image Stolen_Encounter_Image;
    
    public List<GameObject> Player_Controllers = new List<GameObject>();

    [Header("Icons")]
    public Sprite Stolen_Combat_Icon;
    public Sprite Stolen_Overworld_Icon;
    public Sprite[] Sub_Encounter_Icons; // Assign in inspector in order of enum

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Combat_End_Popup.SetActive(false);
        Exit_Button.onClick.AddListener(Close_Combat);   
    }

    public void Close_Combat()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }


    public void Show_Encounter(bool Game_Over)
    {
        //Set the popup content based on encounter type
        switch (Game_Over)
        {
            case true:
                Title.text = "GAME OVER";
                Level_Up_Description.text = "You have died";
                //Stolen_Encounter_Image.sprite = null;
                break;

            case false:
                Title.text = "VICTORY";
                Level_Up_Description.text = "You have NOT died";
               // Stolen_Encounter_Image.sprite = null;
                break;
        }

        Combat_End_Popup.SetActive(true);
        Time.timeScale = 0f; // Pause game
    }
}
