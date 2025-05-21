using JetBrains.Annotations;
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
    public GameObject Enemy_Target;
    public Enemy_Script _Current_Enemy;

    public playerAttributes _Current_Player_Attributes = new playerAttributes();

    public int _Health = 12000;
    public int _Range = 1;
    public int Damage = 15;


    public int Gold_Gained = 0;
    public int XP_Gained = 0;

    public Button _Move_Button, _Attack_Button, _Defend_Button;
     

    private void Start()
    {
        Current_Turn = Player;
        _Move_Button.onClick.AddListener(Move);
        _Attack_Button.onClick.AddListener(Attack);
        _Defend_Button.onClick.AddListener(Defend);
    }

    private void Set_Enemies()
    {
        Enemy_List = _Turn_Order.Get_Enemies();
    }


    // Update is called once per frame
    void Update()
    {
        Movement_Remaining.text = "Movement Remaining = " + Speed.ToSafeString();
        //StartCoroutine(Move_Camera()

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
                }
                else if (Hit_Target.collider.CompareTag("Combat_Enemy"))
                {
                    Enemy_Target = Hit_Target.collider.GameObject();
                    Debug.Log($"Enemy Target Name: {Enemy_Target}");
                }
            }
        }

        if (Current_Turn != Player)
        {
            Current_Tile = Find_Tile(Player);
            while (Current_Turn != Player)
            {
                End_Turn();
                Current_Turn = _Turn_Order.Next_Turn(Current_Turn);
            }
            Debug.Log($"Movement Diactivated");
            Speed = 6;
        }
    }


    public void Move()
    {
        if (Enemy_List.Count == 0)
        { Set_Enemies(); }
        if (Target_Tile != null)
        {
            Debug.Log($"Buttton pressed");
            //if (_Health <= 0)
            //{
            //    Combat_Over.Instance.Show_Encounter(true);
            //}
           

            Current_Tile = Find_Tile(Player);

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



            if (Speed == 0)
            {
                Target_Tile = null;
                Current_Turn = _Turn_Order.Next_Turn(Current_Turn);
                Debug.Log($"End Player Turn");
            }
        }
        else
        { Debug.Log($"Select tile before moving"); }
    }


    IEnumerator Move_To_Tile(Combat_Tile_Script Target_Tile)
    {
        Vector3 Start_Pos = Current_Tile.transform.position;
        Vector3 End_Pos = Target_Tile.transform.position;
        Debug.Log($"target tile transform: {Target_Tile.transform.position}");


        End_Pos.y = Start_Pos.y; // Maintain player's height
        Player.transform.position = End_Pos; // Moves Player's Position


        StartCoroutine(Move_Camera( Target_Tile ));
        yield return null;
    }

    IEnumerator Move_Camera(Combat_Tile_Script Target_Tile)
    {
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

    public void Attack()
    {
        if (Enemy_List.Count == 0)
        { Set_Enemies(); }
        if (Enemy_Target != null)
        {
            if (Current_Tile == null) { Current_Tile = Find_Tile(Player); }
            Target_Tile = Find_Tile(Enemy_Target);
            if (Vector3.Distance(Current_Tile.transform.position, Target_Tile.transform.position) <= _Range)
            {                
                for (int i = 0; i < Enemy_List.Count; i++)
                {
                    if (Enemy_Target == Enemy_List[i])
                    {
                        Enemy_Script _Current_Enemy = Enemy_List[i].GetComponentInChildren<Enemy_Script>();

                        if (_Current_Enemy.Take_Damage(Damage))
                        {
                            Debug.Log($"Enemy Removed");
                            Gold_Gained += _Current_Enemy.Gold;
                            XP_Gained += _Current_Enemy.Experience;
                            Enemy_List[i].SetActive(false);
                            Enemy_List.RemoveAt(i);
                        }
                    }
                }
                Speed = 0;
            }
            else
            {
                Target_Tile = null;
                Debug.Log($"Target tile is out of range");
            }
            if (Enemy_List.Count == 0)
            {
                

                Combat_Over.Instance.Show_Encounter(false);
            }
        }
    }

    public void Defend()
    {
        if (Enemy_List.Count == 0)
        { Set_Enemies(); }
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

    public Combat_Tile_Script Find_Tile(GameObject _Obj)
    {
        Ray Starting_Ray = new UnityEngine.Ray(_Obj.transform.position + Vector3.up, new Vector3(0, -5, 0));
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
        return Current_Tile;
    }

}