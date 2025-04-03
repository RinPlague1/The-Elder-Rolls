using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            Player.transform.Translate(new Vector3(0,0,1));
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            Player.transform.Translate(new Vector3(-1, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            Player.transform.Translate(new Vector3(0, 0, -1));
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            Player.transform.Translate(new Vector3(1, 0, 0));
        }
    }
}
