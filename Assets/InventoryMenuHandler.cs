using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryMenuHandler : MonoBehaviour
{
    [Header("Content Box and Template")]
    public GameObject ContentBox;
    public GameObject Template;

    [Header("Random Item Generation")]
    // Would be nice to have a dictionary for this
    public string[] RandomNames = {"Healing Potion","Mana Potion","Cosmic Shard","Beer","Coke","19 Dollar Fortnite card"};
    public string[] RandomTypes = {"Consumable", "Key Item", "Monster Part"};

    private void Start()
    {
        GenerateInventory(5);
    }

    public void EmptyInventory()
    {
        for (int i=0; i< ContentBox.transform.childCount; i++)
        {
            Destroy(ContentBox.transform.GetChild(i).gameObject);
        }
    }

    public void CreateNewTemplate(string Name,string Type,int NumberHeld)
    {
        GameObject NewItem = GameObject.Instantiate(Template, ContentBox.transform);
        NewItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Name;
        NewItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Type;
        NewItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = NumberHeld.ToString();
    }

    //Testing Function
    public void GenerateInventory(int InventorySize)
    {
        // Clear Current Panes
        EmptyInventory();

        for (int i=0;i< InventorySize; i++)
        {
            CreateNewTemplate(RandomNames[Random.Range(0, RandomNames.Length-1)],RandomTypes[Random.Range(0,RandomTypes.Length-1)],Random.Range(1,101));
        }
    }



}
