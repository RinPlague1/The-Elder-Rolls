using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Player_Move : MonoBehaviour
{
    public GameObject Player;
    public GameObject Enemy;
    public Text Ui;
    private int Speed = 6;
    // Start is called before the first frame update
    void Start()
    {
        Generate_Combat();



      //      Player.transform.position = new Vector3(,1,);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            if (Check_Move())
            {
                Player.transform.Translate(new Vector3(0, 0, 1));
            }
            else {
               Debug.Log("Cannot move outside bounds");
            }
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            Player.transform.Translate(new Vector3(-1, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            Player.transform.Translate(new Vector3(0, 0, -1));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Player.transform.Translate(new Vector3(1, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Check_Adjacent())
            {

                Destroy(Enemy);
            }
        }
        else if (Input.anyKeyDown)
        {
            Debug.Log("Key Not Bound");
        }
    }

    bool Check_Move()
    {
        if (Input.GetKeyDown(KeyCode.W) & Player.transform.position.z >= 3)
        {
            return false;
        }
        else 
        {
            return true;
        }
    }
    bool Check_Adjacent()
    {
        
        if (Math.Abs(Player.transform.position.x - Enemy.transform.position.x) == 1)
        {
            if (Math.Abs(Player.transform.position.z - Enemy.transform.position.z) == 1) {

                return true;
            }
        }
        Debug.Log("Enemy Not Close enough to attack");
        return false;
    }
    void Generate_Combat()
    {
        System.Random rnd = new System.Random();
        Speed = rnd.Next(1, 6);
        

    }

}
