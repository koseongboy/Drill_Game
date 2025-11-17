using UnityEngine;

[CreateAssetMenu(fileName = "New Facility_Data_", menuName = "GameData/Facility_Data_")]
public class Facility_Data_ : ScriptableObject
{
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
    public float Coordinate;
}
