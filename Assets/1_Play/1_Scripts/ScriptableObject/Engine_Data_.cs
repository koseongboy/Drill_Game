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
    public string Type;
    public int Level;
    public List<string> Coordinates;
    public string Desc;
    
    
    public List<Tuple<int, int>> GetCoordinates()
    {
        List<Tuple<int, int>> coordinates = new List<Tuple<int, int>>();
        foreach (string tuple in Coordinates)
        {
            var str = tuple.Split(',');
            coordinates.Add(new Tuple<int, int>(int.Parse(str[0]), int.Parse(str[1])));
        }
        return coordinates;
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
