using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private HexGrid hexGrid;
    private HexTileScript currentTile;
    public GameObject meshRenderer;

    HexTileScript targetTile;

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
                meshRenderer.transform.position = tile.transform.position + Vector3.up * 0.5f; // Adjust height if needed
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
                    targetTile = hit.collider.GetComponent<HexTileScript>();
                    Debug.Log($"TargetTile name: {targetTile.name}");

                    int currentX;
                    int currentY;

                    string stopAt = ",";

                    int charLocation = targetTile.name.IndexOf(stopAt, StringComparison.Ordinal);

                    
                    currentX = Int32.Parse((targetTile.name.Substring(4, charLocation)));
                    currentY = Int32.Parse((targetTile.name.Substring(charLocation + 1, targetTile.name.Length -1)));

                    

                    Vector2Int currentCoords = new Vector2Int(currentX,currentY);


                    Debug.Log($"Current Tile: {currentCoords}");

                    Debug.Log($"Current Tile {currentTile.coordinates} has {currentTile.neighbors.Count} neighbors.");
                    foreach (HexTileScript neighbor in currentTile.neighbors)
                    {
                        Debug.Log($"Neighbor Tile: {neighbor.coordinates}");
                    }

                    bool isNeighbor = false;
                    foreach (HexTileScript neighbor in currentTile.neighbors)
                    {
                        Debug.Log("neighbor: " + neighbor.coordinates + " target tile:" + targetTile.coordinates);
                        if (neighbor.coordinates == targetTile.coordinates)  // Direct reference check
                        {
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
        endPos.y = startPos.y; // Maintain player's height

        float elapsedTime = 0f;
        float moveDuration = Vector3.Distance(startPos, endPos) / moveSpeed;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        currentTile = targetTile; // Update current tile
    }
}