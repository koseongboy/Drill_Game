using System;
using DrillGame._1_Play._1_Scripts.UIScripts.FullWindow;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_ResearchUnit : MonoBehaviour
    {
        [SerializeField]
        private int researchID;

        public int GetResearchID() {
            return researchID;
        }

        public void OnButtonPressed() {
            UI_Research.Instance.OpenDetailWindow( researchID );
        }

        private void OnEnable() {
            if (ResearchManager.Instance.IsResearchUnLocked(researchID)) {
                var color = GetComponent<Image>().color;
                color.a = 1;
                GetComponent<Image>().color = color;
            }
            else {
                var color = GetComponent<Image>().color;
                color.a = 0.5f;
                GetComponent<Image>().color = color;
            }
        }
    }
}
