using DrillGame.Managers;
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

        public void SetItemData(Item_Data_ itemData, int count)
        {
            // Id
            itemId = itemData.GetId();
            
            // Count
            // Count.text = count == 1 
            //     ? "" 
            //     : count.ToString();
            Count.text = count.ToString();
            
            // Sprite
            var path = "Icon/ItemIcon/" + itemData.ItemIcon;
            Sprite icon = Resources.Load<Sprite>(path);
            if (icon == null)
            {
                Debug.LogError("Error: Resources 폴더에서 스프라이트 자원을 찾을 수 없습니다. : "+path);
                return;
            }
            
            Icon.sprite = icon;
        }

        public void OnClick()
        {
            GameManager.Instance.BatchEntity( itemId );
        }

        public void ClearItemData()
        {
            itemId = 0;
            Icon.sprite = null;
            Count.text = "";
        }
    }
}
