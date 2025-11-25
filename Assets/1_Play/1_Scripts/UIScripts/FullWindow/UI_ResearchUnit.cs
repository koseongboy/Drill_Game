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
                GetComponent<Image>().color = new Color(0.87f, 0.87f, 0.87f); //LightGray
                nameText.color = Color.black;
            }
            else {
                GetComponent<Image>().color = new Color(0.18f, 0.18f, 0.18f); //DarkGray
                nameText.color = Color.white;
            }
            
            progressBar.fillAmount = ResearchManager.Instance.GetResearchProgressRate( researchID );
        }
    }
}
