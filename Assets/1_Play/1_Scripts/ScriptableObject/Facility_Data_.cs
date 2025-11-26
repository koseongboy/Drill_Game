using System;
using System.Collections.Generic;
using DrillGame;
using DrillGame._1_Play._1_Scripts.ScriptableObject;
using UnityEngine;

[CreateAssetMenu(fileName = "New Facility_Data_", menuName = "GameData/Facility_Data_")]
public class Facility_Data_ : ScriptableObject, ICSVData
{
    public enum FacilityType
    {
        None = -1,
        Miner = 0,
        Processor = 1,
        Laboratory = 2,
        EngineMerger = 3,
        ResourceMerger = 4,
        Drill = 5
    }
    
    public int GetId()
    {
        return Id;
    }

    public int Id;
    public string Name;
    public string DisplayName;
    public string Type;
    public string Icon;
    public int Level;
    public string EntityClassName;
    public string Desc;
    public int RequireResearchId;
    public int RequireCoreLevel;
    public string BuildResourceId;
    public string BuildResourceCount;
    public string InputItemId;
    public string InputItemCount;
    public int OutputItemId;
    public int OutputItemCount;
    public List<string> Coordinates;
    
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
    
    public FacilityType GetFacilityType_Enum()
    {
        FacilityType returnType = FacilityType.Miner;
        Enum.TryParse(Type, true, out returnType);
        return returnType;
    }
    
    [ContextMenu("Get Coordinates")]
    public void PrintCoordinates_DEV()
    {
        var coordinates = GetCoordinates();
        if (coordinates == null)
        {
            Debug.Log("No coordinates found.");
            return;
        }
        foreach (var tuple in coordinates)
        {
            Debug.Log(tuple.ToString());
        }
    }

}
