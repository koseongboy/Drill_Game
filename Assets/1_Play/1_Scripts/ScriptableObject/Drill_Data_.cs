using DrillGame._1_Play._1_Scripts.ScriptableObject;
using UnityEngine;


[CreateAssetMenu(fileName = "New Drill_Data_", menuName = "GameData/Drill_Data_")]
public class Drill_Data_ : ScriptableObject, ICSVData
{
    public int Id;
    public string Name;
    public string DisplayName;
    public int Damage;
    public string DrillSprite;
  public int GetId()
  {
    return Id;
  }
}
