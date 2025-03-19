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
            gameObject.tag = "HexTile"; // Ensure other tiles have the correct tag
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
}

public enum BiomeType { Grass, Desert, Ocean, Mountain, Forest }