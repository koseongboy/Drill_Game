using DrillGame;
using DrillGame._1_Play._1_Scripts.ScriptableObject;
using UnityEngine;

[CreateAssetMenu(fileName = "New Research_Data_", menuName = "GameData/Research_Data_")]
public class Research_Data_ : ScriptableObject, ICSVData
{
    public int GetId()
    {
        return Id;
    }
    
    public int Id;
    public string DisplayName;
    public int ResearchAmount;
    public int InputItemPerTickId;
    public int InputItemPerTickCount;
    public string RequireResearchId;
}
