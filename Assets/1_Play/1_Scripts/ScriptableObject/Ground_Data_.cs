using System;
using System.Collections.Generic;
using DrillGame;
using DrillGame._1_Play._1_Scripts.ScriptableObject;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ground_Data_", menuName = "GameData/Ground_Data_")]
public class Ground_Data_ : ScriptableObject, ICSVData
{
    public int GetId()
    {
        return Id;
    }
    
    public int Id;
    public string Name;
    public int StartDepth;
    public int EndDepth;
    public int HP;
    public List<string> DropItems;
    public List<string> Color;
    public string SpriteAddressable;

    public Color GetColor()
    {
        float r = Convert.ToInt32(Color[0]) / 255f;
        float g = Convert.ToInt32(Color[1]) / 255f;
        float b = Convert.ToInt32(Color[2]) / 255f;
        return new Color(r, g, b);
    }
}
