using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum PlayerSlot
    {
        Player1,
        Player2,
        Player3,
        Player4
    }

    public enum SceneState
    {
        MainMenu,
        Overworld,
        Combat,
        Video
    }

    [System.Serializable]
    public class PartyMember
    {
        public PlayerSlot slot;
        public playerAttributes attributes;
        public GameObject playerObject;
    }

    public SceneState currentSceneState = SceneState.MainMenu;

    private CharacterCreator.CharacterCreationData characterCreationData;

    private List<CharacterCreator.CharacterCreationData> characterDataList= new List<CharacterCreator.CharacterCreationData>();

    [Tooltip("Assign in inspector or place in Resources folder")]
    public GameObject playerPrefab;


    private List<PartyMember> party = new List<PartyMember>();
    private static GameManager _Instance;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize prefab reference if not set
            if (playerPrefab == null)
            {
                playerPrefab = Resources.Load<GameObject>("Player");
            }
        }
        else if (_Instance != this)
        {
            Destroy(gameObject);
        }
    }




    public static GameManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<GameManager>();
                _Instance.name = _Instance.GetType().ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    public void Update()
    {
        switch (currentSceneState)
        {
            case SceneState.MainMenu:
                if (characterDataList.Count > 3)
                {
                    SceneManager.LoadScene(1);

                    for (int i = 0; i < characterDataList.Count; i++)
                    {
                        Debug.Log($"player{characterDataList[i].characterName} created in OW");
                        CreatePlayerInOverworld(characterDataList[i]);
                    }
                    currentSceneState = SceneState.Overworld;
                }
                break;
        }
    }

    public void SetCharacterCreationData(CharacterCreator.CharacterCreationData data)
    {
        characterCreationData = data;
        characterDataList.Add(data);
    }

    public void AddCharacterToParty(GameObject playerObject, PlayerSlot slot)
    {
        playerAttributes attributes = playerObject.GetComponent<playerAttributes>();
        if (attributes == null)
        {
            Debug.LogError("Player object missing playerAttributes component!");
            return;
        }

        // Check if slot is already occupied
        if (party.Exists(m => m.slot == slot))
        {
            Debug.LogWarning($"Slot {slot} is already occupied!");
            return;
        }

        PartyMember newMember = new PartyMember()
        {
            slot = slot,
            attributes = attributes,
            playerObject = playerObject
        };

        party.Add(newMember);
        DontDestroyOnLoad(playerObject);

        Debug.Log($"Added {attributes.playerName} to party as {slot}");
    }

    public PartyMember GetPartyMember(PlayerSlot slot)
    {
        return party.Find(m => m.slot == slot);
    }

    public List<PartyMember> GetFullParty()
    {
        return new List<PartyMember>(party);
    }

    public void ClearParty()
    {
        foreach (var member in party)
        {
            if (member.playerObject != null)
            {
                Destroy(member.playerObject);
            }
        }
        party.Clear();
    }

    public bool IsPartyFull()
    {
        return party.Count >= 4; // Check if we have 4 party members
    }

    public bool IsSlotOccupied(PlayerSlot slot)
    {
        return party.Exists(m => m.slot == slot);
    }

    


    private void Start()
    {
        Debug.Log("GameManager initialized");
    }


    public void CreatePlayerInOverworld(CharacterCreator.CharacterCreationData _incomingInstansiate)
    {
        if (characterCreationData == null)
        {
            Debug.LogError("No character creation data available!");
            return;
        }

        //playerPrefab = Resources.Load<GameObject>("Assets/CharacterPrefabs/Player");


        GameObject playerInstance = Instantiate(playerPrefab);

        // Instantiate the player
        playerAttributes attributes = playerInstance.GetComponent<playerAttributes>();

        // Configure the player
        attributes.playerName = _incomingInstansiate.characterName;
        attributes.playerClass = _incomingInstansiate.playerClass;
        attributes.primaryAttunement = _incomingInstansiate.primaryAttunement;
        attributes.InitializeAttributes();

        // Add to party
        AddCharacterToParty(playerInstance, _incomingInstansiate.assignedSlot);

        // Position the player (you might want to set this based on your overworld)
        playerInstance.transform.position = Vector3.zero;
    }


}