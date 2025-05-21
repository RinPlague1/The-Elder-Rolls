using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerClass
{
    Warrior,
    Mage,
    Rogue,
    Cleric,
    Ranger
}

[System.Serializable]
public enum MagicAttunement
{
    None,
    Galactic,
    Eldritch,
    Necrotic
}

[System.Serializable]
public class playerAttributes : MonoBehaviour
{
    [Header("Basic Info")]
    public string playerName = "Adventurer";
    public PlayerClass playerClass = PlayerClass.Warrior;

    [Header("Vital Stats")]
    public int maxHealth = 100;
    public int currentHealth;
    public int maxMana = 50;
    public int currentMana;
    public int Experinece;
    public int Level;

    [Header("Magic System")]
    public MagicAttunement primaryAttunement = MagicAttunement.None;
    public MagicAttunement secondaryAttunement = MagicAttunement.None;
    public Dictionary<MagicAttunement, int> attunementLevels = new Dictionary<MagicAttunement, int>();

    [Header("Inventory")]
    public List<Item> inventory = new List<Item>();
    public int gold = 100;
    public int inventoryCapacity = 20;

    [Header("Visuals")]
    public Color galacticColor = Color.cyan;
    public Color eldritchColor = Color.magenta;
    public Color necroticColor = Color.green;

    private void Awake()
    {
        InitializeAttributes();
    }

    public void InitializeAttributes()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        // Initialize attunement levels
        foreach (MagicAttunement attunement in System.Enum.GetValues(typeof(MagicAttunement)))
        {
            attunementLevels[attunement] = (attunement == MagicAttunement.None) ? 0 : 1;
        }

        // Class-based starting setup
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                maxHealth = 120;
                maxMana = 20;
                break;
            case PlayerClass.Mage:
                maxHealth = 70;
                maxMana = 100;
                primaryAttunement = MagicAttunement.Galactic;
                break;
            case PlayerClass.Rogue:
                maxHealth = 90;
                maxMana = 40;
                break;
            case PlayerClass.Cleric:
                maxHealth = 100;
                maxMana = 80;
                primaryAttunement = MagicAttunement.Necrotic;
                break;
            case PlayerClass.Ranger:
                maxHealth = 80;
                maxMana = 60;
                break;
        }

        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    public bool TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            return true; // Player died
        }
        return false;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public bool UseMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            return true;
        }
        return false;
    }

    public void RestoreMana(int amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
    }

    public bool AddItem(Item item)
    {
        if (inventory.Count < inventoryCapacity)
        {
            inventory.Add(item);
            return true;
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        inventory.Remove(item);
    }

    public void IncreaseAttunement(MagicAttunement attunement, int amount = 1)
    {
        if (attunement != MagicAttunement.None)
        {
            attunementLevels[attunement] = Mathf.Min(attunementLevels[attunement] + amount, 10);
        }
    }

    public Color GetAttunementColor(MagicAttunement attunement)
    {
        return attunement switch
        {
            MagicAttunement.Galactic => galacticColor,
            MagicAttunement.Eldritch => eldritchColor,
            MagicAttunement.Necrotic => necroticColor,
            _ => Color.white
        };
    }

    public float GetAttunementPower(MagicAttunement attunement)
    {
        return attunementLevels.ContainsKey(attunement) ? attunementLevels[attunement] / 10f : 0f;
    }

    public bool Level_Up(int Needed)
    {
        if (Experinece >= Needed)
        {
            Experinece -= Needed;
            return true;
        }
        return false;
    }    

}

[System.Serializable]
public class Item
{
    public string itemName;
    public string description;
    public Sprite icon;
    public int value;
    public bool isConsumable;
}