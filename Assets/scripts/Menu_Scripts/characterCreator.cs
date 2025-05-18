using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterCreator : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInput;
    public Dropdown classDropdown;
    public Image characterPreview;
    public Slider attunementSlider;
    public TextMeshProUGUI attunementDescription;
    public Button confirmButton;

    [Header("Visual Options")]
    public List<Sprite> classSprites;
    public Color[] skinColors;
    public Color[] hairColors;
    public GameObject[] hairStyles;

    [Header("Player Prefab")]
    public GameObject playerPrefab;
    public Transform spawnPoint;

    private PlayerClass selectedClass;
    private MagicAttunement primaryAttunement = MagicAttunement.None;
    private int skinColorIndex = 0;
    private int hairStyleIndex = 0;
    private int hairColorIndex = 0;

    private void Start()
    {
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
        UpdateCharacterPreview();
    }

    private void OnClassChanged(int index)
    {
        selectedClass = (PlayerClass)index;
        UpdateCharacterPreview();
        UpdateAttunementOptions();
    }

    private void UpdateAttunementOptions()
    {
        // Different classes have different attunement availability
        switch (selectedClass)
        {
            case PlayerClass.Mage:
                attunementSlider.maxValue = 2;
                break;
            case PlayerClass.Cleric:
                attunementSlider.maxValue = 2;
                break;
            default:
                attunementSlider.maxValue = 1;
                break;
        }
    }

    private void OnAttunementChanged(float value)
    {
        int attunementIndex = Mathf.FloorToInt(value);
        primaryAttunement = (MagicAttunement)(attunementIndex + 1); // Skip "None"

        attunementDescription.text = primaryAttunement switch
        {
            MagicAttunement.Galactic => "Galactic: Space and time magic",
            MagicAttunement.Eldritch => "Eldritch: Forbidden knowledge",
            MagicAttunement.Necrotic => "Necrotic: Life and death powers",
            _ => "No magical attunement"
        };

        UpdateCharacterPreview();
    }

    public void CycleSkinColor()
    {
        skinColorIndex = (skinColorIndex + 1) % skinColors.Length;
        UpdateCharacterPreview();
    }

    public void CycleHairStyle()
    {
        hairStyleIndex = (hairStyleIndex + 1) % hairStyles.Length;
        UpdateCharacterPreview();
    }

    public void CycleHairColor()
    {
        hairColorIndex = (hairColorIndex + 1) % hairColors.Length;
        UpdateCharacterPreview();
    }

    private void UpdateCharacterPreview()
    {
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
            return;
        }

        GameObject newPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        playerAttributes attributes = newPlayer.GetComponent<playerAttributes>();

        // Set basic attributes
        attributes.playerName = nameInput.text;
        attributes.playerClass = selectedClass;
        attributes.primaryAttunement = primaryAttunement;

        // Initialize with selected options
        attributes.InitializeAttributes();

        // Disable creator and enable game
        gameObject.SetActive(false);

        // Here you would typically enable your game controller
        // FindObjectOfType<GameManager>().StartGame(newPlayer);

        Debug.Log($"Character created: {attributes.playerName} the {attributes.playerClass}");
    }
}