using System;
using System.Collections.Generic;
using DrillGame._1_Play._1_Scripts.ScriptableObject;
using UnityEngine;

[CreateAssetMenu(fileName = "New Core_Data_", menuName = "GameData/Core_Data_")]
public class Core_Data_ : ScriptableObject, ICSVData
{
    public int Id;
    public string Name;
    public string DisplayName;
    public int Level;
    public int FacilityCount;
    public int EngineCount;
    public int FactoryLength;
    public List<string> Color;
    public int UpgradeRequiredDepth;
    public int UpgradeRequiredItemId;
    public int UpgradeRequiredItemCount;
    public int GetId()
    {
        return Id;
    }
    
    public Color GetColor()
    {
        float r = Convert.ToInt32(Color[0]) / 255f;
        float g = Convert.ToInt32(Color[1]) / 255f;
        float b = Convert.ToInt32(Color[2]) / 255f;
        return new Color(r, g, b);
    }
}
