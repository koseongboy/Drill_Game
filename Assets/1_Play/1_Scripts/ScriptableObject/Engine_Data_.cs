using System;
using System.Collections.Generic;
using DrillGame;
using DrillGame._1_Play._1_Scripts.ScriptableObject;
using UnityEngine;

[CreateAssetMenu(fileName = "New Engine_Data_", menuName = "GameData/Engine_Data_")]
public class Engine_Data_ : ScriptableObject, ICSVData
{
    public int GetId()
    {
        return Id;
    }
    
    public int Id;
    public int EngineId;
    public string DisplayName;
    public string Icon;
    public string Type;
    public int Level;
    public List<string> Coordinates;
    public string Desc;
    public string Length;
    public string MainCoordinate;
    
    
    public List<Vector2Int> GetCoordinates()
    {
        List<Vector2Int> coordinates = new List<Vector2Int>();
        foreach (string tuple in Coordinates)
        {
            var str = tuple.Split(',');
            coordinates.Add(new Vector2Int(int.Parse(str[0]), int.Parse(str[1])));
        }
        return coordinates;
    }
    public Vector2Int GetLength()
    {
        var str = Length.Split(',');
        return new Vector2Int(int.Parse(str[0]), int.Parse(str[1]));
    }

    public Vector2Int GetMainCoordinate()
    {
        var str = MainCoordinate.Split(',');
        return new Vector2Int(int.Parse(str[0]), int.Parse(str[1]));
    }

    [ContextMenu("Get Coordinates")]
    public void PrintCoordinates_DEV()
    {
        var coordinates = GetCoordinates();
        foreach (var tuple in coordinates)
        {
            Debug.Log(tuple.ToString());
        }
    }

}
