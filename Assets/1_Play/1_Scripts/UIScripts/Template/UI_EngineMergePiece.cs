using System;
using DrillGame._1_Play._1_Scripts.Managers.Mono;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_EngineMergePiece : MonoBehaviour
    {
        [SerializeField] private Image ui_icon;
        [SerializeField] private TextMeshProUGUI ui_name;

        private int level = 0;
        private int targetItemId = 0;
        
        private Action OnButtonPressed;
        public void ButtonPressed()
        {
            OnButtonPressed?.Invoke();    
        }
        
        public void SetData_Level(int level)
        {
            this.level = level;
            ui_icon.sprite = Resources.Load<Sprite>("Icon/ItemIcon/engine_test");
            ui_name.text = $"신규 엔진 Lv.{level}";
            OnButtonPressed = OpenDetail_Level;
        }

        public void SetData_TargetItemId(int targetItemId)
        {
            this.targetItemId = targetItemId;
            
            var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(targetItemId);
            var engineData = ScriptableObjectManager.Instance.GetData<Engine_Data_>(itemData.EntityId);
            
            // Sprite
            var path = "Icon/ItemIcon/" + engineData.Icon;
            Sprite icon = Resources.Load<Sprite>(path);
            if (icon == null)
            {
                Debug.LogError("Error: Resources 폴더에서 스프라이트 자원을 찾을 수 없습니다. : "+path);
                return;
            }
            ui_icon.sprite = icon;
            
            ui_name.text = engineData.DisplayName;
            OnButtonPressed = OpenDetail_Combine;
        }
        
        private void OpenDetail_Level()
        {
            UI_EngineMerger.Instance.OpenDetail( 0, level );
        }

        private void OpenDetail_Combine()
        {
            UI_EngineMerger.Instance.OpenDetail( 1, targetItemId );
        }

    }
}
