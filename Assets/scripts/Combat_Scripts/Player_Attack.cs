using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public Combat_Tile_Script Current_Tile;
    public Combat_Tile_Script Target_Tile;

    private void Move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f); // Draw a red ray

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Combat_Tile"))
                {
                    Target_Tile = null;
                    int Target_X,Target_Y;
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
