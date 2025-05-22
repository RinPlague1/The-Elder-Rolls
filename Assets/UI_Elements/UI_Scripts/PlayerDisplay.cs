using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class PlayerDisplay : MonoBehaviour
{
    // Replace with char info file later
    public int memberIdentifier = 0;
   
    public GameManager.PartyMember electedMember;

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


        electedMember = GameManager.Instance.GetElectedMember(memberIdentifier);

        NameBox.text = electedMember.attributes.playerName;
        HealthBox.text = "HP: " + electedMember.attributes.currentHealth + "/" + electedMember.attributes.maxHealth;
        ManaBox.text = "MP: " + electedMember.attributes.currentMana + "/" + electedMember.attributes.maxMana;
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
