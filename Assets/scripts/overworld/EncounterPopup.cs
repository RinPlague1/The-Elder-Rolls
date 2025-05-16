using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EncounterPopup : MonoBehaviour
{
    public static EncounterPopup Instance { get; private set; }

    [Header("UI References")]
    public GameObject popupPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image encounterIcon;
    public Button confirmButton;
    public PlayerController playerController;

    [Header("Icons")]
    public Sprite combatIcon;
    public Sprite overworldIcon;
    public Sprite[] subEncounterIcons; // Assign in inspector in order of enum

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

        popupPanel.SetActive(false);
        confirmButton.onClick.AddListener(ClosePopup);
    }

    public void ShowEncounter(HexTileScript tile)
    {
        // Set the popup content based on encounter type
        switch (tile.assignedEncounter)
        {
            case HexTileScript.encounterType.none:
                titleText.text = "Safe Area";
                descriptionText.text = "Nothing happens here.";
                //encounterIcon.sprite = null;
                break;

            case HexTileScript.encounterType.combat:
                titleText.text = "Combat Encounter!";
                descriptionText.text = "Prepare for battle!";
                //encounterIcon.sprite = combatIcon;
                break;

            case HexTileScript.encounterType.overworldEncounter:
                titleText.text = GetOverworldEncounterTitle(tile.assignedSubEncounter);
                descriptionText.text = GetOverworldEncounterDescription(tile.assignedSubEncounter);
                //encounterIcon.sprite = overworldIcon;
                // Or use specific icon: encounterIcon.sprite = subEncounterIcons[(int)tile.assignedSubEncounter];
                break;
        }

        popupPanel.SetActive(true);
        Time.timeScale = 0f; // Pause game
    }

    private string GetOverworldEncounterTitle(HexTileScript.subEncounter subEncounter)
    {
        return subEncounter switch
        {
            HexTileScript.subEncounter.healthUp => "Health Boost",
            HexTileScript.subEncounter.healthDown => "Health Drain",
            HexTileScript.subEncounter.goldUp => "Treasure Found",
            HexTileScript.subEncounter.goldDown => "Gold Lost",
            HexTileScript.subEncounter.gainItem => "Item Discovered",
            HexTileScript.subEncounter.upgradeItem => "Item Upgraded",
            _ => "Mysterious Event"
        };
    }

    private string GetOverworldEncounterDescription(HexTileScript.subEncounter subEncounter)
    {
        return subEncounter switch
        {
            HexTileScript.subEncounter.healthUp => "You feel invigorated! Health increased.",
            HexTileScript.subEncounter.healthDown => "You feel weakened. Health decreased.",
            HexTileScript.subEncounter.goldUp => "You found some gold!",
            HexTileScript.subEncounter.goldDown => "You lost some gold...",
            HexTileScript.subEncounter.gainItem => "You found a useful item!",
            HexTileScript.subEncounter.upgradeItem => "One of your items has been upgraded!",
            _ => "Something strange happened..."
        };
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game
        playerController.ResetMoves();
    }
}