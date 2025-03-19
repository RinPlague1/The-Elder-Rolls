using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileScript : MonoBehaviour
{
    public Vector2Int coordinates;
    public BiomeType biome;
    public List<HexTileScript> neighbors = new List<HexTileScript>();

    public void SetCoordinates(Vector2Int coord)
    {
        coordinates = coord;
    }

    public void AssignRandomBiome()
    {
        biome = (BiomeType)Random.Range(0, System.Enum.GetValues(typeof(BiomeType)).Length);

        // Set Ocean and Mountain as barriers
        if (biome == BiomeType.Ocean || biome == BiomeType.Mountain)
        {
            gameObject.tag = "Barrier";
        }
        else
        {
            gameObject.tag = "HexTile";
        }

        GetComponent<Renderer>().material.color = GetBiomeColor();
    }


    Color GetBiomeColor()
    {
        switch (biome)
        {
            case BiomeType.Grass: return Color.green;
            case BiomeType.Desert: return Color.yellow;
            case BiomeType.Ocean: return Color.blue;
            case BiomeType.Mountain: return Color.grey;
            case BiomeType.Forest: return Color.magenta;
            default: return Color.white;
        }
    }

    public void FindNeighbors(Dictionary<Vector2Int, HexTileScript> allTiles)
    {
        neighbors.Clear(); // Clear old neighbors before assigning new ones

        Vector2Int[] possibleOffsets = new Vector2Int[]
        {
        new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(coordinates.y % 2 == 0 ? -1 : 1, 1),
        new Vector2Int(coordinates.y % 2 == 0 ? -1 : 1, -1)
        };

        foreach (var offset in possibleOffsets)
        {
            Vector2Int neighborCoord = coordinates + offset;
            if (allTiles.ContainsKey(neighborCoord))
            {
                HexTileScript neighborTile = allTiles[neighborCoord];

                if (neighborTile.gameObject.tag != "Barrier")  // Ensure it's walkable
                {
                    neighbors.Add(neighborTile);
                    Debug.Log($"Added neighbor: {neighborTile.coordinates} to Tile {coordinates}");
                }
                else
                {
                    Debug.Log($"Skipping barrier tile: {neighborTile.coordinates}");
                }
            }
        }

        Debug.Log($"Tile {coordinates} has {neighbors.Count} neighbors.");
    }



}
public enum BiomeType { Grass, Desert, Ocean, Mountain, Forest}