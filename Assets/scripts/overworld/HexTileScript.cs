using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HexGrid;

public class HexTileScript : MonoBehaviour
{
    public Vector2Int coordinates;
    public BiomeType biome;
    public List<HexTileScript> neighbors = new List<HexTileScript>();

    public void InitializeTile(Vector2Int coords)
    {
        coordinates = coords;
    }

    public void SetNeighbors(List<HexTileScript> neighborTiles)
    {
        neighbors = neighborTiles;
    }

    public void SetBiome(BiomeType newBiome)
    {
        biome = newBiome;
        UpdateTileAppearance();
    }

    void UpdateTileAppearance()
    {
        // Change color based on biome type
        Color tileColor = biome switch
        {
            BiomeType.Ocean => Color.blue,
            BiomeType.Mountain => Color.gray,
            BiomeType.Forest => Color.green,
            BiomeType.Grassland => new Color(0.5f, 1f, 0.5f),
            _ => Color.white
        };

        GetComponent<Renderer>().material.color = tileColor;
    }
}
