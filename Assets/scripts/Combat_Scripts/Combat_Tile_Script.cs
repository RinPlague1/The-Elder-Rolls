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
            Obstacle = Obstacles.None;
        }
        else
        {
             New_Obstacle = UnityEngine.Random.Range(0.0f, 1.0f);
            if (New_Obstacle <= 0.2f)
            {
                Obstacle = Obstacles.Log;
            }
            else if (New_Obstacle > 0.2f && New_Obstacle <= 0.4f)
            {
                Obstacle = Obstacles.Wall;
            }
            else if (New_Obstacle > 0.4f && New_Obstacle <= 0.6f)
            {
                Obstacle = Obstacles.Fence;
            }
            else if (New_Obstacle > 0.6f && New_Obstacle <= 0.8f)
            {
                Obstacle = Obstacles.River;
            }
            else
            {
                Obstacle = Obstacles.Bridge;
            }
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
            _ => Color.white,
        };
        GetComponent<Renderer>().material.color = Tile_Colour;
    }

}
