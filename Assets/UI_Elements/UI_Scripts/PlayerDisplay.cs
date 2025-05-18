using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDisplay : MonoBehaviour
{
    // Replace with char info file later
    public string CharacterName = "CHARNAME";
    public string CharacterHealth = "100";
    public string CharacterMaxHealth = "100";
    public string CharacterMana = "50" ;
    public string CharacterMaxMana = "50" ;
    public Sprite CharacterHead;

    // Get Display Panels
    public TextMeshProUGUI NameBox;
    public TextMeshProUGUI HealthBox;
    public TextMeshProUGUI ManaBox;
    public Image HeadImageBox;

    // Start is called before the first frame update
    void Start()
    {
        NameBox.text = CharacterName;
        HealthBox.text = "HP: " +CharacterHealth + "/" + CharacterMaxHealth;
        ManaBox.text = "MP: " +CharacterMana + "/" + CharacterMaxMana;
        HeadImageBox.sprite = CharacterHead;
    }

    void UpdateName(string NewName)
    {
        NameBox.text = NewName;
    }

    void UpdateHealth(int HP, int MaxHP)
    {
        HealthBox.text = "HP: "+ HP.ToString()+"/"+MaxHP.ToString();
    }

    void UpdateMana(int MP, int MaxMP)
    {
        ManaBox.text = "MP: "+ MP.ToString() + "/" + MaxMP.ToString();
    }

    void UpdateDisplaySprite(Sprite NewSprite)
    {
        HeadImageBox.sprite = NewSprite;
    }
}
