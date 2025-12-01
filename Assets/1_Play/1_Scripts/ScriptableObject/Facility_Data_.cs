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
    public int BuildResourceId;
    public int BuildResourceCount;
    public int InputItemId;
    public int InputItemCount;
    public int OutputItemId;
    public int OutputItemCount;
    public List<string> Coordinates;
    public string Length;
    
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
    
    public FacilityType GetFacilityType_Enum()
    {
        FacilityType returnType = FacilityType.Miner;
        Enum.TryParse(Type, true, out returnType);
        return returnType;
    }

    public string GetFacilityDesc()
    {
        var str = Desc;
        if (OutputItemId != 0)
        {
            str += "\n틱마다 ";
            if (InputItemId != 0)
            {
                var inputItemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(InputItemId);
                str += $"{inputItemData.DisplayName} {InputItemCount}개를 소모해 ";
            }
            var outputItemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(OutputItemId);
            str += $"{outputItemData.DisplayName} {OutputItemCount}개를 생산합니다.";
        }
        return str;
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
