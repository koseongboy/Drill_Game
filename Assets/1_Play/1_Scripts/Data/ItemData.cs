using UnityEngine;
using UnityEngine.Serialization;

namespace DrillGame
{
    [CreateAssetMenu(fileName = "NewResource", menuName = "Drill Game/Create New Item")]
    public class ItemData : ScriptableObject
    {
        [FormerlySerializedAs("resourceID")]
        [Header("Resource Identification")]
        [Tooltip("자원의 고유 ID")]
        public string itemId;       
    
        [Tooltip("게임 내에서 표시될 이름")]
        public string displayName;     

        [Header("Visuals")]
        [Tooltip("UI 인벤토리에 사용될 아이콘")]
        public Sprite itemIcon;             

        // 가공 단계 정의 등 추가 속성
        // public int processingTier;
    }
}