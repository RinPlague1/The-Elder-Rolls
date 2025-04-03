using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexGrid : MonoBehaviour
{

    public GameObject hexPrefab;
    [Header ("map size")]
    public int width = 10;
    public int height = 10;
    public float hexSize = 1f;

    public enum BiomeType { Ocean, Mountain, Forest, Grassland, Desert }


    private Dictionary<Vector2Int, HexTileScript> hexTiles = new Dictionary<Vector2Int, HexTileScript>();

    void Start()
    {
        GenerateGrid();
        GenerateBiomes();
    }

    void GenerateGrid()
    {
        


        float xOffset = hexSize * Mathf.Sqrt(3);
        float yOffset = hexSize * 1.5f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * xOffset, 0, y * yOffset);
                if (y % 2 != 0) position.x += xOffset / 2;

                GameObject hexGO = Instantiate(hexPrefab, position, Quaternion.identity, transform);
                HexTileScript hexTile = hexGO.GetComponent<HexTileScript>();
                Vector2Int coordinates = new Vector2Int(x, y);

                hexTile.InitializeTile(coordinates);
                hexTile.transform.Rotate(-90f, 0f, 0f);
                hexTiles.Add(coordinates, hexTile);
            }
        }

        // Find neighbors for all tiles
        foreach (var tile in hexTiles.Values)
        {
            tile.SetNeighbors(GetNeighbors(tile.coordinates));
        }
    }

    void GenerateBiomes()
    {
        float scale = 10f;
        float offsetX = Random.Range(0f, 1000f);
        float offsetY = Random.Range(0f, 1000f);

        List<HexTileScript> oceanTiles = new List<HexTileScript>();
        List<HexTileScript> mountainTiles = new List<HexTileScript>();

        foreach (var tile in hexTiles.Values)
        {
            float noiseValue = Mathf.PerlinNoise((tile.coordinates.x + offsetX) / scale, (tile.coordinates.y + offsetY) / scale);

            if (noiseValue < 0.3f)
            {
                tile.SetBiome(BiomeType.Ocean);
                oceanTiles.Add(tile);
            }
            else if (noiseValue < 0.4f)
            {
                tile.SetBiome(BiomeType.Mountain);
                mountainTiles.Add(tile);
            }
            else if (noiseValue < 0.6f)
            {
                tile.SetBiome(BiomeType.Forest);
            }
            else
            {
                tile.SetBiome(BiomeType.Grassland);
            }
        }

        ExpandBiome(oceanTiles, BiomeType.Ocean, 2);
        ExpandBiome(mountainTiles, BiomeType.Mountain, 1);
        BalanceGrassland();
    }

    void ExpandBiome(List<HexTileScript> startTiles, BiomeType biome, int expansionSteps)
    {
        HashSet<HexTileScript> biomeTiles = new HashSet<HexTileScript>(startTiles);

        for (int i = 0; i < expansionSteps; i++)
        {
            List<HexTileScript> newTiles = new List<HexTileScript>();

            foreach (var tile in biomeTiles)
            {
                foreach (var neighbor in tile.neighbors)
                {
                    if (!biomeTiles.Contains(neighbor) && Random.value > 0.5f)
                    {
                        neighbor.SetBiome(biome);
                        newTiles.Add(neighbor);
                    }
                }
            }

            biomeTiles.UnionWith(newTiles);
        }
    }

    void BalanceGrassland()
    {
        foreach (var tile in hexTiles.Values)
        {
            int grassNeighbors = 0;
            foreach (var neighbor in tile.neighbors)
            {
                if (neighbor.biome == BiomeType.Grassland)
                    grassNeighbors++;
            }

            if (grassNeighbors > 3 && tile.biome != BiomeType.Ocean && tile.biome != BiomeType.Mountain)
            {
                tile.SetBiome(BiomeType.Grassland);
            }
        }
    }


    List<HexTileScript> GetNeighbors(Vector2Int coordinates)
    {
        List<HexTileScript> neighbors = new List<HexTileScript>();
        Vector2Int[] offsetsEven = { new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(0, 1), new Vector2Int(-1, -1), new Vector2Int(-1, 1) };
        Vector2Int[] offsetsOdd = { new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(0, 1), new Vector2Int(1, -1), new Vector2Int(1, 1) };

        Vector2Int[] offsets = (coordinates.y % 2 == 0) ? offsetsEven : offsetsOdd;

        foreach (var offset in offsets)
        {
            Vector2Int neighborCoords = coordinates + offset;
            if (hexTiles.ContainsKey(neighborCoords))
            {
                neighbors.Add(hexTiles[neighborCoords]);
            }
        }

        return neighbors;
    }

    public HexTileScript GetTileAt(Vector2Int coordinates)
    {
        return hexTiles.ContainsKey(coordinates) ? hexTiles[coordinates] : null;
    }

    public Dictionary<Vector2Int, HexTileScript> GetAllTiles()
    {
        return hexTiles;
    }
}
