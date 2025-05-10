using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Player_Move : MonoBehaviour
{
    public GameObject Player;
    public GameObject Enemy;
    public Text Ui;
    private int Speed = 6;

    public Combat_Tile_Script Current_Tile;
    public Combat_Tile_Script Target_Tile;

    public Camera Player_Camera;

    public GameObject Movement_Container;

    TextMeshPro Movement_Remaining;

    Enemy_Generation Check_Enemy;

    void Start()
    {
          Movement_Remaining = Movement_Container.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray Starting_Ray = new UnityEngine.Ray(Player.transform.position + Vector3.up, new Vector3(0, -5, 0));
        if (Physics.Raycast(Starting_Ray, out RaycastHit Hit_Start))
        {
            if (Hit_Start.collider.CompareTag("Combat_Tile"))
            {
                Current_Tile = null;
                int Current_X, Current_Y;
                Current_Tile = Hit_Start.collider.GetComponent<Combat_Tile_Script>();
                //Debug.Log($"Current Tile Name: {Current_Tile}");

                Current_X = Current_Tile.Coordinates.x;
                // Debug.Log($"Current Tile coord X: {Current_X}");

                Current_Y = Current_Tile.Coordinates.y;
                //Debug.Log($"Current Tile coord Y: {Current_Y}");

            }
        }



        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Button Pressed");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f); // Draw a red ray

            if (Physics.Raycast(ray, out RaycastHit Hit_Target))
            {
                if (Hit_Target.collider.CompareTag("Combat_Tile")) //&& hit.collider.CompareTag("Neighrbour"))
                {
                    Target_Tile = null;
                    int Target_X, Target_Y;
                    Target_Tile = Hit_Target.collider.GetComponent<Combat_Tile_Script>();
                    Debug.Log($"TargetTile name: {Target_Tile}");

                    Target_X = Target_Tile.Coordinates.x;
                    Debug.Log($"target Tile coord X: {Target_X}");

                    Target_Y = Target_Tile.Coordinates.y;
                    Debug.Log($"target Tile coord Y: {Target_Y}");

                    Vector2Int Target_Coord_Vec = Target_Tile.Coordinates;

                    Debug.Log($"target Tile coord vector: {Target_Coord_Vec}");

                   // Debug.Log($"Current Tile {Current_Tile.Coordinates} has {Current_Tile.Neighbours.Count} neighbors.");
                    foreach (Combat_Tile_Script neighbor in Current_Tile.Neighbours)
                    {
                 //       Debug.Log($"Neighbor Tile: {neighbor.Coordinates}");
                    }

                    bool isNeighbor = false;
                    foreach (Combat_Tile_Script neighbor in Current_Tile.Neighbours)
                    {
                     //   Debug.Log("neighbor: " + neighbor.Coordinates + " target tile:" + Target_Coord_Vec);
                        if (neighbor.name == Target_Tile.name)  // Direct reference check
                        {
                       //     Debug.Log("Is a Neighbor");
                            isNeighbor = true;
                            break;
                        }
                    }

                    if (Target_Tile.Coordinates != Check_Enemy.Get_Enemy_Location())
                    {
                        if (Check_Adjacent(Current_Tile.Coordinates, Target_Tile.Coordinates))
                        {

                            StopAllCoroutines();
                            StartCoroutine(Move_To_Tile(Target_Tile));
                            Debug.Log($"Movement Text:  {Movement_Remaining.text}");
                            Movement_Remaining.text = "Movement Remaining = " + Speed.ToSafeString();

                        }
                        else
                        { }
                    }
                    else
                    { Debug.Log($"Tile is not free"); }
                }
            }
        }
    }

    IEnumerator Move_To_Tile(Combat_Tile_Script Target_Tile)
    {
        Vector3 Start_Pos = Current_Tile.transform.position;
        Vector3 End_Pos = Target_Tile.transform.position;
        Debug.Log($"target tile transform: {Target_Tile.transform.position}");


        End_Pos.y = Start_Pos.y; // Maintain player's height
        Player.transform.position = End_Pos; // Moves Player's Position


        End_Pos.y = Player_Camera.transform.position.y; // Maintains camera height
        Player_Camera.transform.position = End_Pos; // Moves Camera with player
        return null;
    }

    bool Check_Adjacent(Vector2 Current_Coords, Vector2 Target_Coords)
    {
        if (Math.Abs(Vector2.Distance(Current_Coords, Target_Coords)) < Speed)
        {
           Speed -= (int)Vector2.Distance(Current_Coords, Target_Coords);
            Debug.Log($"Tile in range");
            return true;
        }
        Debug.Log($"Tile NOT in range");
        return false;
    }

}