using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterCreator : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInput;
    public TMP_Dropdown classDropdown;
    public Image characterPreview;
    public TMP_Dropdown attunementSlider;
    //public TextMeshProUGUI attunementDescription;
    public Button confirmButton;
    public Color playerModelColor;

    [Header("Visual Options")]
    public List<Sprite> classSprites;
    public Color[] skinColors;
    public Color[] hairColors;
    public GameObject[] hairStyles;

    [Header("Player Prefab")]
    public GameObject playerPrefab;
    
 

    private PlayerClass selectedClass;
    private MagicAttunement primaryAttunement = MagicAttunement.None;
    private int skinColorIndex = 0;
    private int hairStyleIndex = 0;
    private int hairColorIndex = 0;


    [Header("Default Names")]
    public string[] warriorNames = { "Garrick", "Thrain", "Brynn", "Eldric" };
    public string[] mageNames = { "Elyndra", "Veylin", "Syndra", "Mordecai" };
    public string[] rogueNames = { "Rook", "Vex", "Lyssandra", "Talon" };
    public string[] clericNames = { "Theodora", "Alistair", "Meridia", "Percival" };
    public string[] rangerNames = { "Arianna", "Kael", "Rowan", "Sylvana" };


    [Header("Party Management")]
    public GameManager.PlayerSlot assignedSlot;
    public bool addToPartyImmediately = true;

    public class CharacterCreationData
    {
        public string characterName;
        public PlayerClass playerClass;
        public MagicAttunement primaryAttunement;
        // Testing Purpose To Differentiate Models.
        public Color ModelColor;
        public int skinColorIndex;
        public int hairStyleIndex;
        public int hairColorIndex;
        public GameManager.PlayerSlot assignedSlot;
    }

    private CharacterCreationData currentCreationData;



    private void Start()
    {
        currentCreationData = new CharacterCreationData();

        // Initialize class dropdown
        classDropdown.ClearOptions();
        List<string> classNames = new List<string>();
        foreach (PlayerClass pc in System.Enum.GetValues(typeof(PlayerClass)))
        {
            classNames.Add(pc.ToString());
        }
        classDropdown.AddOptions(classNames);

        // Set up event listeners
        classDropdown.onValueChanged.AddListener(OnClassChanged);
        attunementSlider.onValueChanged.AddListener(OnAttunementChanged);
        confirmButton.onClick.AddListener(CreateCharacter);

        // Initial setup
        OnClassChanged(0);
        //UpdateCharacterPreview();
    }

    private void OnClassChanged(int index)
    {
        Debug.Log(index);

        switch(index)
        {
            case 0:
                selectedClass = PlayerClass.Warrior;
               
                break;

            case 1:
                selectedClass = PlayerClass.Mage;
                
                break;

            case 2:
                selectedClass = PlayerClass.Rogue;
                
                break;

            case 3:
                selectedClass = PlayerClass.Cleric;
                
                break;

            case 4:
               selectedClass = PlayerClass.Ranger;
                
                break;
        }

        //UpdateCharacterPreview();

    }

    private string GetRandomName(string[] nameList)
    {
        if (nameList == null || nameList.Length == 0)
            return "Adventurer";

        return nameList[Random.Range(0, nameList.Length)];
    }

    private void OnAttunementChanged(int value)
    {
        

        Debug.Log($"Attunement: {value}");
        primaryAttunement = (MagicAttunement)value;

        switch(value)
        {
            case 0:
                primaryAttunement = MagicAttunement.None;
                break;

            case 1:
                primaryAttunement = MagicAttunement.Galactic;
                break;

            case 2:
                primaryAttunement = MagicAttunement.Eldritch;
                break;

            case 3:
                primaryAttunement = MagicAttunement.Necrotic;
                break;
        }

        //attunementDescription.text = primaryAttunement switch
        //{
        //    MagicAttunement.Galactic => "Galactic: Space and time magic",
        //    MagicAttunement.Eldritch => "Eldritch: Forbidden knowledge",
        //    MagicAttunement.Necrotic => "Necrotic: Life and death powers",
        //    _ => "No magical attunement"
        //};

        //UpdateCharacterPreview();
    }

    public void CycleSkinColor()
    {
        skinColorIndex = (skinColorIndex + 1) % skinColors.Length;
        //UpdateCharacterPreview();
    }

    public void CycleHairStyle()
    {
        hairStyleIndex = (hairStyleIndex + 1) % hairStyles.Length;
        //UpdateCharacterPreview();
    }

    public void CycleHairColor()
    {
        hairColorIndex = (hairColorIndex + 1) % hairColors.Length;
        //UpdateCharacterPreview();
    }

    private void UpdateCharacterPreview()
    {
        // Add safety check
        if (classSprites == null || classSprites.Count <= (int)selectedClass)
        {
            Debug.LogError("Missing class sprites or incorrect count!");
            return;
        }
        
        // Update class image
        characterPreview.sprite = classSprites[(int)selectedClass];

        // Update visual effects based on attunement
        if (primaryAttunement != MagicAttunement.None)
        {
            playerAttributes tempAttributes = playerPrefab.GetComponent<playerAttributes>();
            characterPreview.color = tempAttributes.GetAttunementColor(primaryAttunement);
        }
        else
        {
            characterPreview.color = Color.white;
        }

        // Here you would update the 3D model or sprite based on selections
        // This is just a placeholder - you'd need to implement your actual character visuals
    }

    public void CreateCharacter()
    {
        if (string.IsNullOrWhiteSpace(nameInput.text))
        {
            Debug.LogWarning("Please enter a name for your character");
            
            if (selectedClass == PlayerClass.Warrior)
            {
                nameInput.text = GetRandomName(warriorNames);
            }

            if (selectedClass == PlayerClass.Cleric)
            {
                nameInput.text = GetRandomName(clericNames);
            }

            if (selectedClass == PlayerClass.Rogue)
            {
                nameInput.text = GetRandomName(rogueNames);
            }

            if (selectedClass == PlayerClass.Mage)
            {
                nameInput.text = GetRandomName(mageNames);
            }

            if (selectedClass == PlayerClass.Ranger)
            {
                nameInput.text = GetRandomName(rangerNames);
            }


        }
        else
        {
            currentCreationData.characterName = nameInput.text;
        }

        if (GameManager.Instance.IsSlotOccupied(assignedSlot))
        {
            Debug.LogWarning($"Slot {assignedSlot} is already occupied!");
            return;
        }
        GameObject playerPrefabToUse = playerPrefab; // Try Inspector reference first

        //if (playerPrefabToUse == null)
        //{
        //    // Fallback to Resources load if Inspector reference is null
        //    playerPrefabToUse = Resources.Load<GameObject>("PlayerPrefab");

        //    if (playerPrefabToUse == null)
        //    {
        //        Debug.LogError("Player prefab not found! Please either: " +
        //                     "\n1. Assign the prefab in the CharacterCreator Inspector" +
        //                     "\n2. Or place a 'PlayerPrefab.prefab' in a Resources folder");
        //        return;
        //    }
        //}

        // Now safely instantiate
      

        // Store creation data
        currentCreationData = new CharacterCreationData()
        {
            characterName = nameInput.text,
            playerClass = selectedClass,
            primaryAttunement = primaryAttunement,
            ModelColor = playerModelColor,
            skinColorIndex = skinColorIndex,
            hairStyleIndex = hairStyleIndex,
            hairColorIndex = hairColorIndex,
            assignedSlot = assignedSlot
        };

        // Add to GameManager
        GameManager.Instance.SetCharacterCreationData(currentCreationData);

        // Create the character immediately (or you can wait until overworld loads)
        //GameManager.Instance.CreatePlayerInOverworld();

      
        Debug.Log($"Character created: {currentCreationData.characterName} the {currentCreationData.playerClass}");
    }

    public void SetCharacterSlot(int slotIndex)
    {
        assignedSlot = (GameManager.PlayerSlot)slotIndex;
    }

}