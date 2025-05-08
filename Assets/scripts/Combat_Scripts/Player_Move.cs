using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
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

    private void OnDisable()
    {
       
    }

    private void OnEnable()
    {
        Ray Starting_Ray = new UnityEngine.Ray( Player.transform.position, new Vector3(0,-5,0));
        if (Physics.Raycast(Starting_Ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Combat Tile"))
            {
                Current_Tile = null;
                int Current_X, Current_Y;
                Current_Tile = hit.collider.GetComponent<Combat_Tile_Script>();
                Debug.Log($"Current Tile Name: {Current_Tile}");

                Current_X = Current_Tile.Coordinates.x;
                Debug.Log($"Current Tile coord X: {Current_X}");

                Current_Y = Target_Tile.Coordinates.y;
                Debug.Log($"Current Tile coord Y: {Current_Y}");

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Button Pressed");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f); // Draw a red ray

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Combat_Tile")) //&& hit.collider.CompareTag("Neighrbour"))
                {
                    Target_Tile = null;
                    int Target_X, Target_Y;
                    Target_Tile = hit.collider.GetComponent<Combat_Tile_Script>();
                    Debug.Log($"TargetTile name: {Target_Tile}");

                    Target_X = Target_Tile.Coordinates.x;
                    Debug.Log($"target Tile coord X: {Target_X}");

                    Target_Y = Target_Tile.Coordinates.y;
                    Debug.Log($"target Tile coord Y: {Target_Y}");

                    Vector2Int Target_Coord_Vec = Target_Tile.Coordinates;

                    Debug.Log($"target Tile coord vector: {Target_Coord_Vec}");

                    Debug.Log($"Current Tile {Current_Tile.Coordinates} has {Current_Tile.Neighbours.Count} neighbors.");
                    foreach (Combat_Tile_Script neighbor in Current_Tile.Neighbours)
                    {
                        Debug.Log($"Neighbor Tile: {neighbor.Coordinates}");
                    }

                    bool isNeighbor = false;
                    foreach (Combat_Tile_Script neighbor in Current_Tile.Neighbours)
                    {
                        Debug.Log("neighbor: " + neighbor.Coordinates + " target tile:" + Target_Coord_Vec);
                        if (neighbor.name == Target_Tile.name)  // Direct reference check
                        {
                            Debug.Log("Is a Neighbor");
                            isNeighbor = true;
                            break;
                        }
                    }

                    if (isNeighbor)
                    {
                        Debug.Log("Neighbor check PASSED - Moving Player!");
                        StopAllCoroutines();
                        StartCoroutine(Move_To_Tile(Target_Tile));
                    }
                    else
                    {
                        Debug.Log("Neighbor check FAILED - Tile is NOT a neighbor!");
                    }
                }
            }
        }
        //if (Input.GetKeyDown(KeyCode.W)) {
        //    if (Check_Move())
        //    {
        //        Player.transform.Translate(new Vector3(0, 0, 1));
        //    }
        //    else {
        //       Debug.Log("Cannot move outside bounds");
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.A)) {
        //    Player.transform.Translate(new Vector3(-1, 0, 0));
        //}
        //if (Input.GetKeyDown(KeyCode.S)) {
        //    Player.transform.Translate(new Vector3(0, 0, -1));
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Player.transform.Translate(new Vector3(1, 0, 0));
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (Check_Adjacent())
        //    {

        //        Destroy(Enemy);
        //    }
        //}
        //else if (Input.anyKeyDown)
        //{
        //    Debug.Log("Key Not Bound");
        //}
    }

    IEnumerator Move_To_Tile(Combat_Tile_Script targetTile)
    {
        Vector3 Start_Pos = transform.position;
        Vector3 End_Pos = Target_Tile.transform.position;
        Debug.Log($"target tile transform: {targetTile.transform.position}");
        End_Pos.y = Start_Pos.y; // Maintain player's height

        float Elapsed_Time = 0f;
        float Move_Duration = Vector3.Distance(Start_Pos, End_Pos);




        while (Elapsed_Time < Move_Duration)
        {
            //transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            transform.position = End_Pos;

            Elapsed_Time += Time.deltaTime;
            yield return null;
        }

        transform.position = End_Pos;
    }
}


//    bool Check_Move()
//    {
//        if (Input.GetKeyDown(KeyCode.W) & Player.transform.position.z >= 3)
//        {
//            return false;
//        }
//        else 
//        {
//            return true;
//        }
//    }
//    bool Check_Adjacent()
//    {
        
//        if (Math.Abs(Player.transform.position.x - Enemy.transform.position.x) == 1)
//        {
//            if (Math.Abs(Player.transform.position.z - Enemy.transform.position.z) == 1) {

//                return true;
//            }
//        }
//        Debug.Log("Enemy Not Close enough to attack");
//        return false;
//    }
//    void Generate_Combat()
//    {
//        System.Random rnd = new System.Random();
//        Speed = rnd.Next(1, 6);
        

//    }

//}
