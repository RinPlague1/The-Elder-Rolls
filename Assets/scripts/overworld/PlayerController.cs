using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private HexGrid hexGrid;
    public HexTileScript currentTile;

    public HexTileScript targetTile;

    void Start()
    {
        hexGrid = FindObjectOfType<HexGrid>();
        SetInitialPosition();
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f); // Draw a red ray

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("HexTile"))
                {
                    targetTile = null;
                    int targetX;
                    int targetY;
                    targetTile = hit.collider.GetComponent<HexTileScript>();
                    Debug.Log($"TargetTile name: {targetTile}");

                    targetX = targetTile.coordinates.x;
                    Debug.Log($"target Tile coord X: {targetX}");

                    targetY = targetTile.coordinates.y;
                    Debug.Log($"target Tile coord Y: {targetY}");

                    Vector2Int targetCoordVec = targetTile.coordinates;

                    Debug.Log($"target Tile coord vector: {targetCoordVec}");

                    Debug.Log($"Current Tile {currentTile.coordinates} has {currentTile.neighbors.Count} neighbors.");
                    foreach (HexTileScript neighbor in currentTile.neighbors)
                    {
                        Debug.Log($"Neighbor Tile: {neighbor.coordinates}");
                    }

                    bool isNeighbor = false;
                    foreach (HexTileScript neighbor in currentTile.neighbors)
                    {
                        Debug.Log("neighbor: " + neighbor.coordinates + " target tile:" + targetCoordVec);
                        if (neighbor.name == targetTile.name)  // Direct reference check
                        {
                            Debug.Log("Is a Neighbor");
                            isNeighbor = true;
                            break;
                        }
                    }

                    if (isNeighbor)
                    {
                        Debug.Log("Neighbor check PASSED - Moving Player!");
                        StopAllCoroutines();
                        StartCoroutine(MoveToTile(targetTile));
                    }
                    else
                    {
                        Debug.Log("Neighbor check FAILED - Tile is NOT a neighbor!");
                    }
                }
            }
        }
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

            if (UnityEngine.Random.value < tile.encounterChance)
            {
                TriggerEncounter(tile);
            }
        }

        currentTile = tile; // Track where the player is
    }

    private void TriggerEncounter(HexTileScript tile)
    {
        Debug.Log($"Encounter triggered at tile {tile.coordinates} with biome: {tile.biome}");

        Debug.Log($"Encounter triggered at tile {tile.coordinates} Encounter Type: Combat");

        // TODO: You could open a battle scene, show a popup, play a sound, etc.

        //+money -money +health -health +item +upgrade, combat
    }


}