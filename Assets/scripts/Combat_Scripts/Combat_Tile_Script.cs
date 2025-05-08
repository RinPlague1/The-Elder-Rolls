using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Combat_Setup;
public class Combat_Tile_Script : MonoBehaviour
{
    public Vector2Int Coordinates;

    public Obstacles Obstacle;

    public List<Combat_Tile_Script> Neighbours = new List<Combat_Tile_Script>();

    public void Initialize_Tile(Vector2Int Coords)
    {
        float Random_Val = UnityEngine.Random.Range(0.0f, 1.0f);
        Set_Obstacles(Random_Val);

        Coordinates = Coords;
      
    }

    public void Set_Neighbours(List<Combat_Tile_Script> Neighrbour_Tiles)
    {
        Neighbours = Neighrbour_Tiles;
    }

    public void Set_Obstacles(float New_Obstacle)
    {
        if (New_Obstacle <= 0.8)
        {
         //   Obstacle = new Obstacles()
        }
        Update_Tile_Appearance();
    }

    void Update_Tile_Appearance()
    {
        // Replace with Object Prefabs here as they are created

        Color Tile_Colour = Obstacle switch
        {
            Obstacles.Log => Color.red,
            Obstacles.Wall => Color.gray,
            Obstacles.Fence => Color.yellow,
            Obstacles.River => Color.blue,
            Obstacles.Bridge => Color.black,
            Obstacles.None => Color.green,
        };
        GetComponent<Renderer>().material.color = Tile_Colour;
    }

}
