using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour
{
    public GameObject hexPrefab;
    public int width = 10;
    public int height = 10;
    public float hexSize = 1f;
    private Dictionary<Vector2Int, HexTileScript> hexTiles = new Dictionary<Vector2Int, HexTileScript>();

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float xOffset = hexSize * Mathf.Sqrt(3);
        float yOffset = hexSize * 1.5f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xPos = x * xOffset;
                if (y % 2 == 1) xPos += xOffset / 2;

                Vector2Int hexCoord = new Vector2Int(x, y);
                GameObject hex = Instantiate(hexPrefab, new Vector3(xPos, 0, y * yOffset), hexPrefab.transform.rotation, transform);
                hex.name = $"Hex {x},{y}";

                HexTileScript hexTile = hex.AddComponent<HexTileScript>();
                hexTile.SetCoordinates(hexCoord);
                hexTile.AssignRandomBiome();

                hexTiles[hexCoord] = hexTile;
            }
        }
    }
}
