using System;
using DrillGame;
using DrillGame._1_Play._1_Scripts.ScriptableObject;
using DrillGame.Core.Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item_Data_", menuName = "GameData/Item_Data_")]
public class Item_Data_ : ScriptableObject, ICSVData
{
    public int GetId()
    {
        return Id;
    }
    
    public int Id;
    public string Name;
    public string DisplayName;
    public string ItemType;
    public string EngineId;
    public string ItemIcon;

    public InventoryManager.ItemType GetItemType_Enum()
    {
        InventoryManager.ItemType returnType = InventoryManager.ItemType.None;
        Enum.TryParse(ItemType, true, out returnType);
        return returnType;
    }
}
