using DrillGame.Managers;
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

        public async void UpdateUI(int itemId, int count)
        {
            var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(itemId);
            
            ui_name.text = itemData.DisplayName;                
            ui_count.text = count == 0 ? "" : count.ToString();
            
            ui_icon.sprite = await SpriteLoader.Instance.LoadSprite(itemData.ItemIcon);
        }
    }
}