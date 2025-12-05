using DrillGame.Managers;
using DrillGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_ItemSlot : MonoBehaviour
    {
        [SerializeField] private int itemId;
        [SerializeField] private Image Icon;
        [SerializeField] private TextMeshProUGUI Count;

        public void SetItemData(Item_Data_ itemData, int count = 0)
        {
            // Id
            itemId = itemData.GetId();
            
            Count.text = count == 0
                ? "" 
                : count.ToString();
            
            // Sprite
            var icon = SpriteLoader.Instance.LoadSprite(itemData.ItemIcon);
            Icon.sprite = icon.Result;
        }

        public void OnClick()
        {
            UILoader.Instance.ShowUI_ItemDetail( itemId );
        }

        public void ClearItemData()
        {
            itemId = 0;
            Icon.sprite = null;
            Count.text = "";
        }
    }
}
