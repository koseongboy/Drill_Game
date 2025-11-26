using System;
using DrillGame._1_Play._1_Scripts.UIScripts.FullWindow;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_ResearchUnit : MonoBehaviour
    {
        [SerializeField]
        private int researchID;

        [SerializeField] private string text;
        [SerializeField]
        private TextMeshProUGUI ui_nameText;
        [SerializeField]
        private Image progressBar;

        public int GetResearchID() {
            return researchID;
        }

        public void OnButtonPressed() {
            UI_Research.Instance.OpenDetailWindow( researchID );
        }

        public void SetData(int researchId, string name)
        {
            this.researchID = researchId + transform.GetSiblingIndex();
            ui_nameText.text = name + "\n"+ text;
        }

        public void UpdateUI()
        {
            var image = GetComponent<Image>();
            
            if (ResearchManager.Instance.IsResearchUnLocked(researchID)) { // Unlock
                image.color = new Color(0.87f, 0.87f, 0.87f); //LightGray
                ui_nameText.color = Color.black;
            }else if (researchID == ResearchManager.Instance.GetSelectedResearchId()) // Selected
            {
                image.color = new Color(0.87f, 0.4f, 0f); //Orange
                ui_nameText.color = Color.white;
            }else {
                image.color = new Color(0.18f, 0.18f, 0.18f); //DarkGray
                ui_nameText.color = Color.white;
            }
            
            // Research Progress
            progressBar.fillAmount = ResearchManager.Instance.GetResearchProgressRate( researchID );
        }

        private void Awake()
        {
            UI_Research.Instance.AddResearchUnitToDict( this );
        }

        private void OnEnable()
        {
            UpdateUI();
        }
    }
}
