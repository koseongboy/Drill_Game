using System;
using DrillGame.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_ContentPiece : MonoBehaviour
    {
        [SerializeField] private Image ui_icon;
        [SerializeField] private TextMeshProUGUI ui_name;
        
        private Action OnButtonPressed;
        
        public void ButtonPressed()
        {
            OnButtonPressed?.Invoke();    
        }
        
        public void SetData(string name, string iconName, Action OnButtonPressed)
        {
            ui_name.text = name.ToString();
            ui_icon.sprite = SpriteLoader.Instance.LoadSprite(iconName).Result;
            
            this.OnButtonPressed = OnButtonPressed;
        }
    }
}
