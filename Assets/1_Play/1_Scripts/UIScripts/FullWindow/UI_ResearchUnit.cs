using System;
using DrillGame._1_Play._1_Scripts.UIScripts.FullWindow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_ResearchUnit : MonoBehaviour
    {
        [SerializeField]
        private int researchID;
        [SerializeField]
        private TextMeshProUGUI nameText;
        [SerializeField]
        private Image progressBar;

        public int GetResearchID() {
            return researchID;
        }

        public void OnButtonPressed() {
            UI_Research.Instance.OpenDetailWindow( researchID );
        }

        private void OnEnable() {
            if (ResearchManager.Instance.IsResearchUnLocked(researchID)) {
                var color = GetComponent<Image>().color;
                color = Color.white;
                GetComponent<Image>().color = color;
                
                nameText.color = Color.black;
            }
            else {
                var color = GetComponent<Image>().color;
                color = Color.black;
                GetComponent<Image>().color = color;
                
                nameText.color = Color.white;
            }
            
            progressBar.fillAmount = ResearchManager.Instance.GetResearchProgressRate( researchID );
        }
    }
}
