using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    public Combat_Setup _Setup;
    public Player_Move _Move;
    public GameObject _Player;
    public Combat_Turn_Order _Turn_Order;
    public int _Speed;
    List<GameObject> Enemies;
    public playerAttributes _Player_Attributes;

    public void Get_Enemies(List<GameObject> Enemy_List)
    {
        Enemies = Enemy_List;
    }
    GameObject Get_Current_Turn()
    {
        return _Move.Current_Turn;
    }
    GameObject Get_Player()
    {
        return _Move.Player;
    }
    Combat_Tile_Script Get_Player_Tile()
    {
        return _Move.Current_Tile;
    }

    public void Enemy_Movement(GameObject _Enemy)
    {
        _Player = Get_Player();


        Combat_Tile_Script Enemy_Tile;
        Ray Enemy_Ray = new UnityEngine.Ray(_Enemy.transform.position + Vector3.up, new Vector3(0, -5, 0));
        if (Physics.Raycast(Enemy_Ray, out RaycastHit Hit_Start))
        {
            if (Hit_Start.collider.CompareTag("Combat_Tile"))
            {
                Enemy_Tile = Hit_Start.collider.GetComponent<Combat_Tile_Script>();


                if (Vector2.Distance(Enemy_Tile.Coordinates, Get_Player_Tile().Coordinates) > _Speed)
                {
                  
                }
                else
                {
                    if (Vector3.Cross(_Player.transform.position, _Enemy.transform.position).y < 0)
                    {
                        _Enemy.transform.position = _Player.transform.position + Vector3.left;
                    }
                    else
                    {
                        _Enemy.transform.position = _Player.transform.position + Vector3.right;
                    }



                    _Enemy.transform.position = _Player.transform.position + new Vector3(1,0,0);
                }
            }
        }
        _Move.Current_Turn = _Turn_Order.Next_Turn(_Move.Current_Turn);
    }

}
