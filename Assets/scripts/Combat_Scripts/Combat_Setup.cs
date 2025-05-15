using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Combat_Setup;

public class Combat_Setup : MonoBehaviour
{
    public GameObject Combat_Tile_Prefab;
    public GameObject Enemy_Prefab;
    [Header("Combat Map Size")]
    public int Width;
    public int Height;
    public float Tile_Size;

    public enum Obstacles { Log, Wall, Fence, River, Bridge, None}

    public GameObject Player;

    public List<GameObject> Enemies = new List<GameObject>();

    private Dictionary<Vector2Int, Combat_Tile_Script> Combat_Tiles = new Dictionary<Vector2Int, Combat_Tile_Script>();

    Combat_Turn_Order Turn_Order;

    Enemy_Generation Generate;


    void Set_Initial_Player_Position(Vector2Int Coords)
    {
        Player.transform.position = new UnityEngine.Vector3(0.05f +Coords.x, 0,0.05f + Coords.y);

        for (int i = 0; i < Enemies.Count; i++) 
        {
           

            GameObject Enemy_GO = Instantiate(Enemy_Prefab, Generate.Set_Enemy_Position(new Vector2Int(UnityEngine.Random.Range(0, Width), UnityEngine.Random.Range(0, Height))), Quaternion.identity, transform);
            Enemy_Script Enemy = Enemy_GO.GetComponent<Enemy_Script>();

            Enemies.Add(Enemy_GO);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Generate_Combat_Grid();
        Set_Initial_Player_Position(new Vector2Int (0,0));
    }

    void Generate_Combat_Grid()
    {
        float X_Offset = Tile_Size;
        float Y_Offset = Tile_Size;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector3 Position = new Vector3(x * X_Offset, 0, y * Y_Offset);
                GameObject TileGO = Instantiate(Combat_Tile_Prefab, Position, Quaternion.identity, transform);
                Combat_Tile_Script Combat_Tile = TileGO.GetComponent<Combat_Tile_Script>();
                Vector2Int Coordinates = new Vector2Int(x, y);

                Combat_Tile.Initialize_Tile(Coordinates);
                Combat_Tile.transform.Rotate(0f, 0f, 0f);
                Combat_Tiles.Add(Coordinates, Combat_Tile);
            }
            foreach (var Tile in Combat_Tiles.Values)
            {
                Tile.Set_Neighbours(Get_Neighbours(Tile.Coordinates));
            }
        }

    }

    List<Combat_Tile_Script> Get_Neighbours (Vector2Int Coordinates)
    {
        List<Combat_Tile_Script> Neighbours = new List<Combat_Tile_Script>();
        for (int x = -1; x < 2; x++) {
            for (int y = -1; y < 2; y++)
            {
                Vector2Int Neighbour_Coords = Coordinates + new Vector2Int (x , y); // + Offset
                if (Combat_Tiles.ContainsKey(Neighbour_Coords))
                {
                    Neighbours.Add(Combat_Tiles[Neighbour_Coords]);
                }
            }
        }
        return Neighbours;
    }

    public Combat_Tile_Script Get_Tile_At(Vector2Int Coordinates)
    {
        return Combat_Tiles.ContainsKey(Coordinates) ? Combat_Tiles[Coordinates] : null;
    }

    public Dictionary<Vector2Int, Combat_Tile_Script> Get_All_Tiles()
    {
        return Combat_Tiles;
    }
}

