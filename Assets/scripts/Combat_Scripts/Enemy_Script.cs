using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public enum Enemy_Type
{
    Melee,
    Ranged,
    Boss
}

[System.Serializable]
public enum Enemy_Attunement
{
    None,
    Galactic,
    Eldritch,
    Necrotic
}

public class Enemy_Script : MonoBehaviour
{
    [Header("Basic Info")]
    public Enemy_Type Enemy_Class = Enemy_Type.Melee;


    [Header("Vital Stats")]
    public int Max_Health = 10;
    public int Current_Health;
    public int Max_Mana = 20;
    public int Current_Mana;
    public int Experience = 5;

    [Header("Magic System")]
    public Enemy_Attunement Primary = Enemy_Attunement.None;
    public Enemy_Attunement Secondary = Enemy_Attunement.None;
    public Dictionary<Enemy_Attunement, int> Attunement_Levels = new Dictionary<Enemy_Attunement, int>();

    [Header("Inventory")]
    public List<Item> Inventory = new List<Item>();
    public int Gold = 10;
    public int Capacity = 20;

    [Header("Visuals")]
    public Color Galactic_Color = Color.cyan;
    public Color Eldritch_Color = Color.magenta;
    public Color Necrotic_Color = Color.green;



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
                        StartCoroutine(Delay_Action(1f));
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
            if (Is_Player[i] == Player_Tile && !_Move.Defending) 
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




    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        foreach (Enemy_Attunement Attunement in System.Enum.GetValues(typeof(Enemy_Attunement)))
        {
            Attunement_Levels[Attunement] = (Attunement == Enemy_Attunement.None) ? 0 : 1;
        }

        switch (Enemy_Class)
        {
            case Enemy_Type.Melee:
                Max_Health = 30;
                Max_Mana = 10;
                break;
            case Enemy_Type.Ranged:
                Max_Health = 10;
                Max_Mana = 30;
                break;

            case Enemy_Type.Boss:
                Max_Health = 200;
                Max_Mana = 200;
                break;

        }
        Current_Health = Max_Health;
        Current_Mana = Max_Mana;
    }

    public bool Take_Damage(int _Amount)
    {
        Current_Health -= _Amount;
        if (Current_Health <= 0)
        {
            Current_Health = 0;
            return true;
        }
        return false;
    }

    public void Heal(int _Amount) { Mathf.Min(Current_Health + _Amount, Max_Health); }

    public bool Use_Mana(int _Amount)
    {
        if (Current_Mana >= _Amount)
        {
            Current_Mana -= _Amount;
            return true;
        }
        return false;
    }

    public void Restore_Mana(int _Amount) { Mathf.Min(Current_Mana + _Amount, Max_Mana); }

    public Color Get_Attunement_Color(Enemy_Attunement _Attunement)
    {
        return _Attunement switch
        {
            Enemy_Attunement.Galactic => Galactic_Color,
            Enemy_Attunement.Necrotic => Necrotic_Color,
            Enemy_Attunement.Eldritch => Eldritch_Color,
            _ => Color.white
        };
    }

    public float Get_Attunement_Power(Enemy_Attunement _Attunemnt)
    {
        return Attunement_Levels.ContainsKey(_Attunemnt) ? Attunement_Levels[_Attunemnt] / 10f : 0f;
    }


    [System.Serializable]
    public class Item
    {
        public string Item_Name;
        public string Description;
        public Sprite Icon;
        public int Value;
        public bool Consumable;
    }

}
