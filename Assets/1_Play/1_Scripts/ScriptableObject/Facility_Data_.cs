using System;
using System.Collections.Generic;
using DrillGame;
using DrillGame._1_Play._1_Scripts.ScriptableObject;
using UnityEngine;

[CreateAssetMenu(fileName = "New Facility_Data_", menuName = "GameData/Facility_Data_")]
public class Facility_Data_ : ScriptableObject, ICSVData
{
    public int GetId()
    {
        return Id;
    }

    public int Id;
    public string Name;
    public string DisplayName;
    public string Type;
    public int Level;
    public string BuildResourceId;
    public string BuildResourceCount;
    public string InputItemId;
    public string InputItemCount;
    public int OutputItemId;
    public int OutputItemCount;
    public List<string> Coordinates;
    
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
