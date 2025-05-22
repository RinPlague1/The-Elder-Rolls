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

    private HexGrid overworldGrid;

    [Tooltip("Assign in inspector or place in Resources folder")]
    public GameObject playerPrefab;

    private int currentActiveMemberIndex = 0;
    private float switchCooldown = 0.5f;
    private float lastSwitchTime = 0f;


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

            case SceneState.Overworld:
                if (currentSceneState == SceneState.Overworld && Input.GetKeyDown(KeyCode.Space))
{
                    if (Time.time - lastSwitchTime > switchCooldown)
                    {
                        CycleActivePartyMember();
                        lastSwitchTime = Time.time;
                    }
                }


                break;
        }
    }

    public void CycleActivePartyMember()
    {
        if (party.Count == 0) return;

        // Check if current player can switch
        PlayerController currentController = party[currentActiveMemberIndex].playerObject.GetComponent<PlayerController>();
        if (currentController != null && !currentController.CanSwitch())
        {
            return;
        }

        // Deactivate current member
        if (currentController != null)
        {
            currentController.SetAsActivePlayer(false);
            party[currentActiveMemberIndex].attributes.movesLeft = party[currentActiveMemberIndex].attributes.maxMoves;
            currentController.RegisterSwitch();
        }

        // Move to next member
        currentActiveMemberIndex = (currentActiveMemberIndex + 1) % party.Count;

        // Activate new member
        PlayerController nextController = party[currentActiveMemberIndex].playerObject.GetComponent<PlayerController>();
        if (nextController != null)
        {
            nextController.SetAsActivePlayer(true);
            nextController.RegisterSwitch();

            // Force camera update
            if (nextController.isActivePlayer)
            {
                CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
                if (cameraFollow != null)
                {
                    cameraFollow.SetTarget(nextController.transform);
                }
            }
        }

        Debug.Log($"Now controlling: {party[currentActiveMemberIndex].attributes.playerName}");
    }

    public PlayerController GetActivePlayer()
    {
        if (party.Count == 0 || currentActiveMemberIndex >= party.Count)
            return null;

        return party[currentActiveMemberIndex].playerObject.GetComponent<PlayerController>();
    }

    public void SetOverworldGrid(HexGrid grid)
    {
        overworldGrid = grid;
    }

    public HexGrid GetOverworldGrid()
    {
        return overworldGrid;
    }

    public HexTileScript GetTileAt(Vector2Int coordinates)
    {
        if (overworldGrid != null)
        {
            return overworldGrid.GetTileAt(coordinates);
        }
        return null;
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

    public PartyMember GetElectedMember(int _electedPlace)
    {
        return party[_electedPlace];
    }

    public Dictionary<Vector2Int, HexTileScript> GetAllTiles()
    {
        if (overworldGrid != null)
        {
            return overworldGrid.GetAllTiles();
        }
        return new Dictionary<Vector2Int, HexTileScript>();
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
        playerAttributes attributes = playerInstance.GetComponent<playerAttributes>();

        // Configure the player
        attributes.playerName = _incomingInstansiate.characterName;
        attributes.playerClass = _incomingInstansiate.playerClass;
        attributes.primaryAttunement = _incomingInstansiate.primaryAttunement;
        attributes.InitializeAttributes();

        // Add to party
        AddCharacterToParty(playerInstance, _incomingInstansiate.assignedSlot);

        HexGrid grid = GetOverworldGrid();
        if (grid != null)
        {
            Dictionary<Vector2Int, HexTileScript> allTiles = grid.GetAllTiles();
            foreach (var tile in allTiles.Values)
            {
                if (tile.gameObject.tag != "Barrier")
                {
                    playerInstance.transform.position = tile.transform.position + Vector3.up * 0.5f;
                    PlayerController controller = playerInstance.GetComponent<PlayerController>();
                    if (controller != null)
                    {
                        controller.currentTile = tile;
                        attributes.currentTile = tile;
                    }
                    break;
                }
            }
        }


        // Position the player (you might want to set this based on your overworld)
        playerInstance.transform.position = Vector3.zero;

        if (party.Count == 0)
        {
            PlayerController controller = playerInstance.GetComponent<PlayerController>();
            if (controller != null) controller.SetAsActivePlayer(true);
        }

    }


    public void ResetAllMoves()
    {
        foreach (PartyMember member in party)
        {
            if (member.attributes != null)
            {
                member.attributes.movesLeft = member.attributes.maxMoves;
            }
        }
    }

}