using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public bool isActivePlayer = false;
    private CameraFollow cameraFollow;

    [Header("UI References")]
    public TextMeshProUGUI movesText;
    public TextMeshProUGUI maxMovesText;
    public TextMeshProUGUI currentTileText;
    public TextMeshProUGUI targetTileText;

    [Header("Active Player Settings")]
    public GameObject activeIndicator; // Visual indicator for active player
    public float switchCooldown = 0.2f; // Cooldown between switches
    private float lastSwitchTime = 0f;

    [Header("Movement Settings")]
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 5.0f;

    [Header("Tile References")]
    [SerializeField] private HexTileScript _currentTile;
    [SerializeField] private HexTileScript _targetTile;

    private HexGrid hexGrid;
    private playerAttributes playerAttrib;
    private bool isMoving = false;

    // Public properties with validation
    public HexTileScript currentTile
    {
        get => _currentTile;
        set
        {
            _currentTile = value;
            if (playerAttrib != null) playerAttrib.currentTile = value;
            UpdateTileUI();
        }
    }

    public HexTileScript targetTile
    {
        get => _targetTile;
        set
        {
            _targetTile = value;
            UpdateTileUI();
        }
    }

    void Awake()
    {
        Instance = this;
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    void Start()
    {
        playerAttrib = GetComponent<playerAttributes>();
        if (playerAttrib == null)
        {
            Debug.LogError("PlayerAttributes component missing from player object!");
            return;
        }
        StartCoroutine(GetHexGridLayout());

        
    }

    private void FindUIReferences()
    {
        // Find the canvas that contains our UI elements
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in scene!");
            return;
        }

        // fuck you chat gippity we do it manually
        movesText = canvas.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        

        if (movesText == null) Debug.LogWarning("MovesDisplayText not found in UI");
        
    }

    private TextMeshProUGUI FindUIText(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                Debug.Log("child found");
                return child.GetComponent<TextMeshProUGUI>();
            }

            // Search recursively in children
            TextMeshProUGUI foundInChildren = FindUIText(child, name);
            if (foundInChildren != null)
            {
                return foundInChildren;
            }
        }
        return null;
    }

    void UpdateTileUI()
    {
        if (currentTileText != null)
        {
            currentTileText.text = currentTile != null ?
                $"Current: {currentTile.coordinates}\nBiome: {currentTile.biome}" :
                "Current: None";
        }

        if (targetTileText != null)
        {
            targetTileText.text = targetTile != null ?
                $"Target: {targetTile.coordinates}\nBiome: {targetTile.biome}" :
                "Target: None";
        }
    }

    void UpdateMovesUI()
    {
        if (movesText != null && playerAttrib != null)
        {
            Debug.Log($"Moves: {playerAttrib.movesLeft}/{playerAttrib.maxMoves}");
            movesText.text = $"Moves: {playerAttrib.movesLeft}/{playerAttrib.maxMoves}";
        }
    }

    void SetInitialPosition()
    {
        var validTiles = GameManager.Instance.GetAllTiles()
            .Where(t => t.Value.gameObject.tag != "Barrier")
            .Select(t => t.Value)
            .ToList();

        if (validTiles.Count > 0)
        {
            // Find the closest valid tile to the center
            HexTileScript spawnTile = validTiles
                .OrderBy(t => Vector2.Distance(t.coordinates, new Vector2(hexGrid.width / 2, hexGrid.height / 2)))
                .First();

            transform.position = spawnTile.transform.position + Vector3.up * 0.5f;
            currentTile = spawnTile;
            targetTile = null;
        }
        else
        {
            Debug.LogError("No valid spawn tiles found!");
        }
    }

    void Update()
    {
        if (!isActivePlayer || isMoving) return;

        if (Input.GetMouseButtonDown(0))
        {
            HandleTileSelection();
        }
    }

    void HandleTileSelection()
    {
        if (playerAttrib == null || playerAttrib.movesLeft <= 0)
        {
            Debug.Log("No moves left or player attributes missing!");
            return;
        }

        if (Camera.main == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider == null || !hit.collider.CompareTag("HexTile"))
            {
                Debug.Log("No valid tile selected");
                return;
            }

            HexTileScript selectedTile = hit.collider.GetComponent<HexTileScript>();
            if (selectedTile == null)
            {
                Debug.Log("Selected tile has no HexTileScript component");
                return;
            }

            if (currentTile == null)
            {
                Debug.LogWarning("Current tile is null - resetting position");
                SetInitialPosition();
                return;
            }

            if (IsValidMove(selectedTile))
            {
                targetTile = selectedTile;
                playerAttrib.movesLeft--;
                UpdateMovesUI();
                StartCoroutine(MoveToTile(targetTile));
            }
        }
    }

    bool IsValidMove(HexTileScript destination)
    {
        // Check if destination is a neighbor of current tile
        bool isNeighbor = currentTile.neighbors.Contains(destination);

        if (destination.tag == "HexTile")
        {
            return isNeighbor;
        }
        return false;
    }

    IEnumerator MoveToTile(HexTileScript destination)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = destination.transform.position + Vector3.up * 0.5f;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(endPos - startPos);

        float elapsedTime = 0f;
        float moveDuration = Vector3.Distance(startPos, endPos) / moveSpeed;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        transform.rotation = endRot;

        currentTile = destination;
        targetTile = null;
        OnStepOntoTile(destination);
        isMoving = false;
    }

    private void OnStepOntoTile(HexTileScript tile)
    {
        
        if (!tile.beenVisited)
        {
            tile.beenVisited = true;
            TriggerEncounter(tile);
        }
    }

    private void TriggerEncounter(HexTileScript tile)
    {
        Debug.Log($"Encounter triggered at tile {tile.coordinates} with biome: {tile.biome}");
        Debug.Log($"Encounter Type: {tile.assignedEncounter}");

        if (tile.assignedEncounter == HexTileScript.encounterType.overworldEncounter)
        {
            Debug.Log($"Sub Encounter: {tile.assignedSubEncounter}");
        }

        if (playerAttrib.movesLeft == 0)
        {
            EncounterPopup.Instance.ShowEncounter(tile);
        }
    }

    public void ResetMoves()
    {
        if (playerAttrib != null)
        {
            playerAttrib.movesLeft = playerAttrib.maxMoves;
            //UpdateMovesUI();
        }
    }

    public void SetAsActivePlayer(bool active)
    {
        isActivePlayer = active;

        // Toggle visual indicator
        if (activeIndicator != null)
        {
            activeIndicator.SetActive(active);
        }

        // Set camera target
        if (active)
        {
            // Ensure we have a camera follow reference
            if (cameraFollow == null)
            {
                cameraFollow = Camera.main.GetComponent<CameraFollow>();
                if (cameraFollow == null)
                {
                    Debug.LogError("No CameraFollow component found on main camera!");
                    return;
                }
            }

            cameraFollow.SetTarget(transform);
        }

        // Update UI immediately
       // UpdateTileUI();
        UpdateMovesUI();

        // Enable/disable input components if needed
        var inputHandler = GetComponent<PlayerInputHandler>();
        if (inputHandler != null)
        {
            inputHandler.enabled = active;
        }
    }

    public bool CanSwitch()
    {
        return Time.time - lastSwitchTime > switchCooldown;
    }

    public void RegisterSwitch()
    {
        lastSwitchTime = Time.time;
    }

    public void SetControlledCharacter(playerAttributes attributes)
    {
        playerAttrib = attributes;
        //UpdateMovesUI();
        //UpdateTileUI();
    }

    // Debug method to visualize current and target tiles
    void OnDrawGizmosSelected()
    {
        if (currentTile != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(currentTile.transform.position + Vector3.up, 0.5f);
        }

        if (targetTile != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetTile.transform.position + Vector3.up, 0.5f);
        }
    }

    private IEnumerator GetHexGridLayout()
    {
        yield return new WaitForFixedUpdate();

        FindUIReferences();
        hexGrid = GameManager.Instance.GetOverworldGrid();
        if (hexGrid == null)
        {
            Debug.LogError("HexGrid not found in GameManager!");
            yield break;
        }

        SetInitialPosition();
        UpdateMovesUI();
        UpdateTileUI();
    }
}