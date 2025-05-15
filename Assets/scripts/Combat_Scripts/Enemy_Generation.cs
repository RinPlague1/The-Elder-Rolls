using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Generation : MonoBehaviour
{
    public GameObject Enemy;

    public Combat_Tile_Script Enemy_Tile;

    private void Start()
    {
        Enemy_Tile = null;
    }

    public UnityEngine.Vector3 Set_Enemy_Position(Vector2Int Coords)
    {
        UnityEngine.Vector3 enemyTransform = new UnityEngine.Vector3(0.05f + Coords.x, 0, 0.05f + Coords.y);

        return enemyTransform;
    }



    public Vector2Int Get_Enemy_Location()
    {
        Ray Enemy_Ray = new UnityEngine.Ray(Enemy.transform.position + Vector3.up, new Vector3(0, -5, 0));
        if (Physics.Raycast(Enemy_Ray, out RaycastHit Hit_Start))
        {
            if (Hit_Start.collider.CompareTag("Combat_Tile"))
            {
                Enemy_Tile = Hit_Start.collider.GetComponent<Combat_Tile_Script>();
            }
        }
        return Enemy_Tile.Coordinates;
    }
}
