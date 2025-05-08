using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public Combat_Tile_Script Current_Tile;
    public Combat_Tile_Script Target_Tile;


    private void Update()
    {
        
    }


    private void Move()
    {
        
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
