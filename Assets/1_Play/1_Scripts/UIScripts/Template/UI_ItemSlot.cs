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

        public async void SetItemData(Item_Data_ itemData, int count = 0)
        {
            // Id
            itemId = itemData.GetId();
            
            Count.text = count == 0
                ? "" 
                : count.ToString();
            
            // Sprite
            Icon.sprite = await SpriteLoader.Instance.LoadSprite(itemData.ItemIcon);
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
