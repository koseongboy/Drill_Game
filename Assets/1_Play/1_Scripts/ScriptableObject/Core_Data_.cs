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
    public int UpgradeRequiredDepth;
    public int UpgradeRequiredItemId;
    public int UpgradeRequiredItemCount;
    public int GetId()
    {
        return Id;
    }
}
