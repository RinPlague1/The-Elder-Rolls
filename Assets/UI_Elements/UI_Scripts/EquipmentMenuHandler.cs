using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentMenuHandler : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI CharacterNameText;
    public TextMeshProUGUI WeaponText;
    public TextMeshProUGUI ArmorText;
    public TextMeshProUGUI MagicText;
    public Image BackgroundColorImage;

    [Header("Defaults")]
    public string DefaultName;
    public string DefaultWeapon;
    public string DefaultArmor;
    public string DefaultMagic;
    public Color[] DefaultColors;

    [Header("Player Colors")]
    public Color P1Color;
    public Color P2Color;
    public Color P3Color;
    public Color P4Color;

    // Needs a reference to each player attributes
    // [Header ("Player Attributes")]
    // public PlayerAttributes P1Info = null;
    // public PlayerAttributes P2Info = null;
    // public PlayerAttributes P3Info = null;
    // public PlayerAttributes P4Info = null;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateSelected(DefaultName, DefaultWeapon, DefaultArmor, DefaultMagic, DefaultColors[Random.Range(0,DefaultColors.Length - 1)]);
    }

    private void UpdateSelected(string PlayerName, string WeaponName, string ArmorName, string MagicName, Color PortColor)
    {
        CharacterNameText.text = $"{PlayerName}";
        WeaponText.text = $"Weapon: {WeaponName}";
        ArmorText.text = $"Armor: {ArmorName}";
        MagicText.text = $"Magic: {MagicName}";
        BackgroundColorImage.color = PortColor;
    }

    public void UpdateToPlayer1()
    {
        // Change values to P1 Info in here
        UpdateSelected("Player 1", "Hammer", "Chainmail", "None", P1Color);
    }

    public void UpdateToPlayer2()
    {
        // Change values to P2 Info
        UpdateSelected("Player 2", "Longsword", "Cloth", "Cosmic", P2Color);
    }

    public void UpdateToPlayer3()
    {
        // Change values to P3 Info
        UpdateSelected("Player 3", "Bow", "Cloak", "Necrotic", P3Color);
    }

    public void UpdateToPlayer4()
    {
        // Change values to P4 Info
        UpdateSelected("Player 4","Mage stick"," Mage Robe","Eldritch",P4Color);
    }

}
