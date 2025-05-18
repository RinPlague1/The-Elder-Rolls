using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    public Combat_Setup _Setup;
    public Player_Move _Move;
    public GameObject _Player;
    public int _Speed;
    List<GameObject> Enemies;

    private void Start()
    {
        Enemies = Get_Enemies();
    }


    void Update()
    {
        foreach (GameObject Enemy in Enemies)
        {
            if (Enemy == Get_Current_Turn())
            {
                _Player = Get_Player();
                Enemy.transform.position = _Player.transform.position;
            }
        }
    }

    List<GameObject> Get_Enemies()
    { 
        return _Setup.Enemies;
    }
    GameObject Get_Current_Turn()
    {
        return _Move.Current_Turn;
    }
    GameObject Get_Player()
    {
        return _Move.Player;
    }


    Vector3 Enemy_Movement(GameObject _Enemy)
    {
        float _Distance = Vector3.Distance(_Player.transform.position, _Enemy.transform.position);

        if (_Distance < _Speed)
        {
            //Enemy Attack
        }
        else
        {
          //  _Enemy.transform.position = 
        }

        return _Enemy.transform.position;
    }

}
