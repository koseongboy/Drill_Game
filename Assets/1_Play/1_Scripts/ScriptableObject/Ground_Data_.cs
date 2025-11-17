using UnityEngine;

[CreateAssetMenu(fileName = "New Ground_Data_", menuName = "GameData/Ground_Data_")]
public class Ground_Data_ : ScriptableObject
{
    public int Id;
    public string Name;
    public int StartDepth;
    public int EndDepth;
    public int HP;
    public string DropItems;
    public string SpriteAddressable;
}
