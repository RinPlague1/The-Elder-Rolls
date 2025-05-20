using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_Script : MonoBehaviour
{
    public Combat_Setup _Setup;
    public Player_Move _Move;
    public GameObject _Player;
    public Combat_Turn_Order _Turn_Order;
    public int _Speed;
    List<GameObject> Enemies;
    private playerAttributes _Player_Attributes;
    private int _Health;

    public A_Star_Pathfinding _Pathfinding;
    public  List<Combat_Tile_Script> _Path;

    public GameObject _Container;

    private void Set_Setup()
    {
        _Setup = _Container.GetComponentInChildren<Combat_Setup>();
        _Turn_Order = _Container.GetComponentInChildren<Combat_Turn_Order>();
    } 


    public void Get_Enemies(List<GameObject> Enemy_List)
    {
        Enemies = Enemy_List;
    }
   
    Combat_Tile_Script Get_Player_Tile()
    {
        return _Move.Current_Tile;
    }

    public void Enemy_Movement(GameObject _Enemy, GameObject Container, GameObject Move)
    {
        _Speed = 6;
        _Container = Container;
        _Move = Move.GetComponentInChildren<Player_Move>();
        _Pathfinding = Move.GetComponentInChildren<A_Star_Pathfinding>();
        Set_Setup();

        _Player = _Move.Get_Player();

        
        Combat_Tile_Script Enemy_Tile, Player_Tile;

        Player_Tile = _Move.Get_Player_Tile();


        Ray Enemy_Ray = new UnityEngine.Ray(_Enemy.transform.position + Vector3.up, new Vector3(0, -5, 0));
        if (Physics.Raycast(Enemy_Ray, out RaycastHit Hit_Start))
        {
            if (Hit_Start.collider.CompareTag("Combat_Tile"))
            {
                Enemy_Tile = Hit_Start.collider.GetComponent<Combat_Tile_Script>();
                Debug.Log($"Working One: {Enemy_Tile}");
                Debug.Log($"Working One: {Player_Tile}");
                Debug.Log($"Working One: {Container}");
                _Pathfinding.Setup(Enemy_Tile, Player_Tile, Container);
                _Path = _Pathfinding.Find_Path(Enemy_Tile.Coordinates.x, Enemy_Tile.Coordinates.y, Get_Player_Tile().Coordinates.x, Get_Player_Tile().Coordinates.y);
                if (_Path != null)
                {
                    Debug.Log($"Working One");
                    for (int i = 1; i < _Path.Count; i++)
                    {
                        
                        Debug.Log($"Path: {_Path[i].Coordinates}");
                        //StartCoroutine(Delay_Action(1f));
                        StopAllCoroutines();
                        StartCoroutine(Enemy_Move_To_Tile(_Path[i - 1], _Path[i]));
                        Delay_Action(1f);
                        _Speed--;
                        if (0 == _Speed) 
                        { break; }
                    }
                }
                else
                {
                    Debug.Log($"Broken One");
                }
            }
        }


        List<Combat_Tile_Script> Is_Player = _Pathfinding.Get_Adjacent(_Path[_Path.Count - 1]);

        for (int i = 0; i < Is_Player.Count; i++)
        {
            if (Is_Player[i] == Player_Tile) 
            {
                _Move.Set_Health(_Move.Get_Health() - 60);
                //if (_Player_Attributes.TakeDamage(_Player_Attributes.maxHealth))
                //{
                //    break;
                //}
            }
        }
    }

    IEnumerator Delay_Action(float Delay)
    {
        yield return new WaitForSeconds(Delay);
    }



    IEnumerator Enemy_Move_To_Tile(Combat_Tile_Script Current_Tile, Combat_Tile_Script Target_Tile)
    {
        Vector3 Start_Pos = Current_Tile.transform.position;
        Vector3 End_Pos = Target_Tile.transform.position;

        Debug.Log($"target tile transform: {Target_Tile.transform.position}");
        Camera Player_Camera = _Move.Get_Camera();

        End_Pos.y = Player_Camera.transform.position.y; // Maintains camera height
        Player_Camera.transform.position = End_Pos; // Moves Camera with player



        End_Pos.y = Start_Pos.y; // Maintain player's height
        this.gameObject.transform.position = End_Pos; // Moves Player's Position

        End_Pos.y = Player_Camera.transform.position.y; // Maintains camera height
        Player_Camera.transform.position = End_Pos; // Moves Camera with player

        yield return null;
    }


}
