using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class A_Star_Pathfinding :MonoBehaviour
{ 
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;




    public Combat_Setup _Setup;


    private List<Combat_Tile_Script> _Tile_List;

    public  List<Combat_Tile_Script> _Open_List;
    public List<Combat_Tile_Script> _Closed_List;

    public Combat_Tile_Script _Start_Tile;
    public Combat_Tile_Script _End_Tile;

    private int _Width;
    private int _Height;

    public void Setup(Combat_Tile_Script Start_Tile, Combat_Tile_Script End_Tile, GameObject Grid)
    {
        _Start_Tile = Start_Tile;
        _End_Tile = End_Tile;
        _Setup = Grid.GetComponentInChildren<Combat_Setup>();
        _Width = Get_Grid_Width();
        _Height = Get_Grid_Height();
        _Tile_List = _Setup.Get_Tiles();
    }



    int G_Cost, H_Cost, F_Cost;

    public List<Combat_Tile_Script> Find_Path(int Start_X, int Start_Y, int End_X, int End_Y)
    {


        _Open_List = new List<Combat_Tile_Script>() { _Start_Tile };
        _Closed_List = new List<Combat_Tile_Script>();


        for (int i = 0; i < _Tile_List.Count; i++)
        {
            _Tile_List[i].G_Cost = int.MaxValue;
            _Tile_List[i].Calculate_F_Cost();
            _Tile_List[i].Previous_Node = null;
        }


        _Start_Tile.G_Cost = 0;
        _Start_Tile.H_Cost = Calculate_Distance(_Start_Tile, _End_Tile);
        _Start_Tile.Calculate_F_Cost();


        Debug.Log($"Pathfinding Initialized");


        while (_Open_List.Count != 0)
        {
            Combat_Tile_Script Current_Node = Get_Lowest_F_Cost(_Open_List);

            Debug.Log($"Current:  {Current_Node.Coordinates}");
            Debug.Log($"Current:  {_End_Tile.Coordinates}");
            Debug.Log($"Current:  {_Width}");
            Debug.Log($"Current:  {_Height}");


            if (Current_Node.Coordinates == _End_Tile.Coordinates)
            {
                //Reached End Node
                return Calculate_Path(_End_Tile.Previous_Node);
            }
            else
            {
                Debug.Log($"Node Finished: ");
                _Open_List.Remove(Current_Node);
                _Closed_List.Add(Current_Node);

                List<Combat_Tile_Script> Neighbours = Get_Adjacent(Current_Node);

                foreach (Combat_Tile_Script Neighbour in Neighbours)
                {
                    if (Neighbour.Obstacle != Combat_Setup.Obstacles.None)
                    {
                        _Closed_List.Add(Neighbour);
                        continue;
                    }
                    if (_Closed_List.Contains(Neighbour)) { continue; }

                    
                    int Temp_G_Cost = Current_Node.G_Cost + Calculate_Distance(Current_Node, Neighbour);
                    
                    if (Temp_G_Cost < Neighbour.G_Cost)
                    {
                        Debug.Log($"Temp: {Temp_G_Cost}");
                       // Debug.Log($"Neighbour: {Neighbour.G_Cost}");
                        Neighbour.Previous_Node = Current_Node;
                        Neighbour.G_Cost = Temp_G_Cost;
                        Neighbour.H_Cost = Calculate_Distance(Neighbour, _End_Tile);
                        Debug.Log($"H Cost: {Neighbour.H_Cost}");
                        Neighbour.Calculate_F_Cost();


                        if (!_Open_List.Contains(Neighbour))
                        {
                            Debug.Log($"Neighbour ADDED: {Neighbour}");
                            _Open_List.Add(Neighbour);
                        }
                    }
                }
            }

        }
        Debug.Log($"Path not found");
        return null;
    }

    public List<Combat_Tile_Script> Get_Adjacent(Combat_Tile_Script Current)
    {
        List<Combat_Tile_Script> Neighbour_List = new List<Combat_Tile_Script>();

        if (Current.Coordinates.x - 1 >= 0)
        {
            Neighbour_List.Add(Get_Node(Current.Coordinates.x - 1, Current.Coordinates.y));

            if (Current.Coordinates.y - 1 >= 0) Neighbour_List.Add(Get_Node(Current.Coordinates.x - 1, Current.Coordinates.y - 1));
            if (Current.Coordinates.y + 1 < _Height) Neighbour_List.Add(Get_Node(Current.Coordinates.x - 1, Current.Coordinates.y + 1));
        }
        if (Current.Coordinates.x + 1 < _Width)
        {
            Neighbour_List.Add(Get_Node(Current.Coordinates.x + 1, Current.Coordinates.y));

            if (Current.Coordinates.y - 1 >= 0) Neighbour_List.Add(Get_Node(Current.Coordinates.x + 1, Current.Coordinates.y - 1));
            if (Current.Coordinates.y + 1 < _Height) Neighbour_List.Add(Get_Node(Current.Coordinates.x + 1, Current.Coordinates.y + 1));
        }

        if (Current.Coordinates.y - 1 >= 0) Neighbour_List.Add(Get_Node(Current.Coordinates.x , Current.Coordinates.y - 1));
        if (Current.Coordinates.y + 1 < _Height) Neighbour_List.Add(Get_Node(Current.Coordinates.x , Current.Coordinates.y + 1));

        Debug.Log($"Last neighbour ADDED: {Neighbour_List[Neighbour_List.Count - 1]}");
        return Neighbour_List;
    }


    private Combat_Tile_Script Get_Node(int _X, int _Y)
    {
        for (int k = 0; k < _Tile_List.Count ; k++)
        {
            if (_X == _Tile_List[k].Coordinates.x && _Y == _Tile_List[k].Coordinates.y)
                return _Tile_List[k];
        }
        return null;
    }


    private List<Combat_Tile_Script> Calculate_Path(Combat_Tile_Script _End_Node)
    {
        List<Combat_Tile_Script> Path = new List<Combat_Tile_Script>();
        Path.Add(_End_Node);
        Combat_Tile_Script Current = _End_Node;
        while (Current.Previous_Node != null)
        {
            Path.Add(Current.Previous_Node);
            Current = Current.Previous_Node;
        }
        Path.Reverse();
        return Path;
    }

    private int Calculate_Distance(Combat_Tile_Script A, Combat_Tile_Script B)
    {
        int X_Distance = Mathf.Abs(A.Coordinates.x - B.Coordinates.x);
        int Y_Distance = Mathf.Abs(A.Coordinates.y - B.Coordinates.y);
        int Remaining = Mathf.Abs(X_Distance - Y_Distance);
        return DIAGONAL_COST * Mathf.Min(X_Distance,Y_Distance) + STRAIGHT_COST * Remaining;
    }

    private Combat_Tile_Script Get_Lowest_F_Cost(List<Combat_Tile_Script> Path_Node_List)
    {
        Combat_Tile_Script Lowest_Cost = Path_Node_List[0];

        for (int i = 1; i < Path_Node_List.Count; i++)
        {
            if (Path_Node_List[i].F_Cost < Lowest_Cost.F_Cost)
            {
                Lowest_Cost = Path_Node_List[i];
            }
        }
        return Lowest_Cost;
    }



    private int Get_Grid_Width()
    {
        return _Setup.Width;
    }
    private int Get_Grid_Height()
    {
        return _Setup.Height;
    }



}
