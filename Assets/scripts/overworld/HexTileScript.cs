using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HexGrid;



public class HexTileScript : MonoBehaviour
{

    
    public enum encounterType { none, combat, overworldEncounter};
    public enum subEncounter { healthUp, healthDown, goldUp, goldDown, gainItem, upgradeItem };

    public Vector2Int coordinates;
    public BiomeType biome;
    public List<HexTileScript> neighbors = new List<HexTileScript>();
    public bool beenVisited = false;
    public float encounterChance = 0.45f;

    public encounterType assignedEncounter;
    public subEncounter assignedSubEncounter;
    

    public void InitializeTile(Vector2Int coords)
    {
        float randomValue = UnityEngine.Random.Range(0.0f,1.0f);
        Debug.Log($"Random value: {randomValue}");
        setEncounterType(randomValue);


        coordinates = coords;
    }

    public void setEncounterType(float randomValue)
    {
        if (randomValue < 0.5)
        {
            assignedEncounter = encounterType.none;
        }

        if (randomValue >= 0.5 && randomValue < 0.75) 
        {
            assignedEncounter = encounterType.combat;
        }

        if (randomValue >= 0.75 && randomValue < 1)
        {
            assignedEncounter = encounterType.overworldEncounter;
            float subRandomValue = UnityEngine.Random.Range(0.0f, 1.0f);

            if (subRandomValue <= 0.166667)
            {
                assignedSubEncounter = subEncounter.healthUp;
            }

            if (subRandomValue > 0.166667 && subRandomValue <= 0.333333)
            {
                assignedSubEncounter = subEncounter.healthDown;
            }

            if (subRandomValue > 0.333333 && subRandomValue <= 0.5)
            {
                assignedSubEncounter = subEncounter.goldUp;
            }

            if (subRandomValue > 0.5 && subRandomValue <= 0.666667)
            {
                assignedSubEncounter = subEncounter.goldDown;
            }

            if (subRandomValue > 0.666667 && subRandomValue <= 0.833334)
            {
                assignedSubEncounter = subEncounter.gainItem;
            }

            if (subRandomValue > 0.833334 && subRandomValue <= 1)
            {
                assignedSubEncounter = subEncounter.upgradeItem;
            }


        }
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
