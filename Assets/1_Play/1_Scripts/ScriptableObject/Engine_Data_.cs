using UnityEngine;

[CreateAssetMenu(fileName = "New Engine_Data_", menuName = "GameData/Engine_Data_")]
public class Engine_Data_ : ScriptableObject
{
    public int ID;
    public int EngineId;
    public string DisplayName;
    public string Type;
    public int Level;
    public float Coordinates;
    public string Desc;
}
