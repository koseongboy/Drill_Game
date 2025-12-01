using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_ResourceItemPiece : MonoBehaviour
    {
        [SerializeField] private Image ui_icon;
        [SerializeField] private TextMeshProUGUI ui_name;
        [SerializeField] private TextMeshProUGUI ui_count;

        public void UpdateUI(int itemId, int count)
        {
            var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(itemId);
            
            ui_name.text = itemData.DisplayName;                
            ui_count.text = count == 0 ? "" : count.ToString();
            
            // Sprite
            var path = "Icon/ItemIcon/" + itemData.ItemIcon;
            Sprite icon = Resources.Load<Sprite>(path);
            if (icon == null)
            {
                Debug.LogError("Error: Resources 폴더에서 스프라이트 자원을 찾을 수 없습니다. : "+path);
                return;
            }
            
            ui_icon.sprite = icon;
        }
    }
}