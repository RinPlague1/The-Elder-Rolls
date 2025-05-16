using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int maxMoves = 5;
    public int currentMoves;
    public TextMeshProUGUI movesText;

    public float moveSpeed = 1.0f;

    private HexGrid hexGrid;
    public HexTileScript currentTile;

    public HexTileScript targetTile;

    void Start()
    {
        hexGrid = FindObjectOfType<HexGrid>();
        SetInitialPosition();
        currentMoves = maxMoves; // Initialize moves
        UpdateMovesUI();
    }

    void UpdateMovesUI()
    {
        if (movesText != null)
        {
            movesText.text = $"{currentMoves}/{maxMoves}";
        }
    }

    void SetInitialPosition()
    {
        Dictionary<Vector2Int, HexTileScript> allTiles = hexGrid.GetAllTiles();

        // Find the first non-barrier tile
        foreach (var tile in allTiles.Values)
        {
            if (tile.gameObject.tag != "Barrier")
            {
                currentTile = tile;
                transform.position = tile.transform.position + Vector3.up * 0.5f; // Adjust height if needed
                break;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if player has moves left
            if (currentMoves <= 0)
            {
                Debug.Log("No moves left!");
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("HexTile"))
                {
                    targetTile = hit.collider.GetComponent<HexTileScript>();

                    bool isNeighbor = false;
                    foreach (HexTileScript neighbor in currentTile.neighbors)
                    {
                        if (neighbor.name == targetTile.name)
                        {
                            isNeighbor = true;
                            break;
                        }
                    }

                    if (isNeighbor)
                    {
                        currentMoves--; // Deduct move
                        UpdateMovesUI(); // Update UI
                        StopAllCoroutines();
                        StartCoroutine(MoveToTile(targetTile));
                    }
                }
            }
        }
    }

    public void ResetMoves()
    {
        currentMoves = maxMoves;
        UpdateMovesUI();
    }

    IEnumerator MoveToTile(HexTileScript targetTile)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = targetTile.transform.position;
        Debug.Log($"target tile transform: {targetTile.transform.position}");
        endPos.y = startPos.y; // Maintain player's height

        float elapsedTime = 0f;
        float moveDuration = Vector3.Distance(startPos, endPos) / moveSpeed;

        while (elapsedTime < moveDuration)
        {
            //transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            transform.position = endPos;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        OnStepOntoTile(targetTile);
    }


    private void OnStepOntoTile(HexTileScript tile)
    {
        currentTile = targetTile; // Update current tile
        Debug.Log($"current tile reassigned: {currentTile}");
        if (!tile.beenVisited)
        {
            tile.beenVisited = true;
                        
            TriggerEncounter(tile);
            
        }

        currentTile = tile; // Track where the player is
    }

    private void TriggerEncounter(HexTileScript tile)
    {
        Debug.Log($"Encounter triggered at tile {tile.coordinates} with biome: {tile.biome}");
        Debug.Log($"Encounter Type: {tile.assignedEncounter}");

        if (tile.assignedEncounter == HexTileScript.encounterType.overworldEncounter)
        {
            Debug.Log($"Sub Encounter: {tile.assignedSubEncounter}");
        }

        // Show the popup

        if (currentMoves == 0)
        { 
        EncounterPopup.Instance.ShowEncounter(tile);
    
        }
    }

}