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

    public GameObject Movement_Manager;
    private int Speed = 6;

    public Combat_Tile_Script Current_Tile;
    public Combat_Tile_Script Target_Tile;

    public Camera Player_Camera;

//    public GameObject Movement_Container;

    public TextMeshProUGUI Movement_Remaining;
    public Combat_Turn_Order _Turn_Order;
    public GameObject Current_Turn;

    public List<GameObject> Enemy_List;
    public Enemy_Script _Current_Enemy;

    public playerAttributes _Current_Player_Attributes = new playerAttributes();

    public int _Health = 120;

    private void Start()
    {
        Current_Turn = Player;
    }

    private void Set_Enemies()
    {
        Enemy_List =  _Turn_Order.Get_Enemies();
    }


    // Update is called once per frame
    void Update()
    {

        Set_Enemies();
        if (_Health <= 0)
        {
            Combat_Over.Instance.Show_Encounter(true);
        }
        if (Enemy_List.Count == 0)
        {
            Combat_Over.Instance.Show_Encounter(false);
        }

         Movement_Remaining.text = "Moves: " + Speed.ToSafeString();
        if (Current_Turn == Player)
        {

            Ray Starting_Ray = new UnityEngine.Ray(Player.transform.position + Vector3.up, new Vector3(0, -5, 0));
            if (Physics.Raycast(Starting_Ray, out RaycastHit Hit_Start))
            {
                if (Hit_Start.collider.CompareTag("Combat_Tile"))
                {
                    Current_Tile = null;
                    int Current_X, Current_Y;
                    Current_Tile = Hit_Start.collider.GetComponent<Combat_Tile_Script>();
                    Current_X = Current_Tile.Coordinates.x;
                    Current_Y = Current_Tile.Coordinates.y;

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


                        Target_Y = Target_Tile.Coordinates.y;


                        Vector2Int Target_Coord_Vec = Target_Tile.Coordinates;


                        bool Checker = true;
                        if (Checker)
                        {
                            if (Check_Adjacent(Current_Tile.Coordinates, Target_Tile.Coordinates))
                            {

                                StopAllCoroutines();
                                StartCoroutine(Move_To_Tile(Target_Tile));
                            }
                            else
                            { }
                        }
                        else
                        { Debug.Log($"Tile is not free"); }
                    }
                }
            }
            if (Speed == 0)
            {
                Current_Turn = _Turn_Order.Next_Turn(Current_Turn);
                Debug.Log($"End Player Turn");
            }

        }
        else
        {
            Ray Starting_Ray = new UnityEngine.Ray(Player.transform.position + Vector3.up, new Vector3(0, -5, 0));
            if (Physics.Raycast(Starting_Ray, out RaycastHit Hit_Start))
            {
                if (Hit_Start.collider.CompareTag("Combat_Tile"))
                {
                    Current_Tile = null;
                    int Current_X, Current_Y;
                    Current_Tile = Hit_Start.collider.GetComponent<Combat_Tile_Script>();
                    Current_X = Current_Tile.Coordinates.x;
                    Current_Y = Current_Tile.Coordinates.y;

                }
            }

            while (Current_Turn != Player)
            {
                End_Turn();
                Current_Turn = _Turn_Order.Next_Turn(Current_Turn);
            }
            Debug.Log($"Movement Diactivated");
            Speed = 6;
            Movement_Manager.SetActive(false);
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
        yield return null;
    }

    IEnumerator Move_Camera(Combat_Tile_Script Target_Tile)
    {
        Vector3 Start_Pos = Current_Tile.transform.position;
        Vector3 End_Pos = Target_Tile.transform.position;

        End_Pos.y = Player_Camera.transform.position.y; // Maintains camera height
        Player_Camera.transform.position = End_Pos; // Moves Camera with player
        yield return null;
    }

    bool Check_Adjacent(Vector2 Current_Coords, Vector2 Target_Coords)
    {
        if (Math.Abs(Vector2.Distance(Current_Coords, Target_Coords)) <= Speed)
        {
           Speed -= (int)Vector2.Distance(Current_Coords, Target_Coords);
            Debug.Log($"Tile in range");
            return true;
        }
        Debug.Log($"Tile NOT in range");
        return false;
    }

    private void Player_Attack()
    {
       
    }

    private void End_Turn()
    {
        
        for (int i = 0; i < Enemy_List.Count; i++)
        {
            if (Enemy_List[i] == Current_Turn)
            {
                _Current_Enemy = Enemy_List[i].GetComponentInChildren<Enemy_Script>();
                _Current_Enemy.Enemy_Movement(Enemy_List[i], _Turn_Order.gameObject, this.gameObject);
                break;
            }
        }
        Debug.Log($"Current Turn: {Current_Turn}");
    }

    public GameObject Get_Player()
    {
        return Player;
    }

    public Combat_Tile_Script Get_Player_Tile()
    {
        return Current_Tile;
    }

    public Camera Get_Camera()
    {
        return Player_Camera;
    }

    public int Get_Health()
    {
        return _Health;
    }
    public void Set_Health(int Health)
    {
       _Health = Health;
    }


    public playerAttributes Get_Player_Attributes()
    {
        return _Current_Player_Attributes;
    }
}