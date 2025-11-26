using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_BuildPiece : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ui_name;
        [SerializeField] private Image ui_icon;

        private int itemId = 0;
        
        public void SetData(int facilityItemId)
        {
            itemId = facilityItemId;
            var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>( itemId );
            var data = ScriptableObjectManager.Instance.GetData<Facility_Data_>( itemData.EntityId );
            ui_name.text = data.DisplayName;
            
            // Sprite
            if (data.Icon != "" || data.Icon != null)
            {
                Sprite icon = Resources.Load<Sprite>("Icon/ItemIcon/" + data.Icon);
                if (icon == null)
                {
                    Debug.LogError($"Error: Resources 폴더에서 스프라이트 자원을 찾을 수 없습니다. : {data.Icon}");
                }
                ui_icon.sprite = icon;
            }
        }

        public void OnButtonPressed()
        {
            Debug.Log($"OnButtonPressed : {itemId}");
        }
    }
}
