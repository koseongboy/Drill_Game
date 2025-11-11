using System;
using DG.Tweening;
using DrillGame.UI;
using DrillGame.UI.Interface;
using UnityEngine;
using TMPro;

namespace DrillGame
{
    public class UI_FloatingBar : MonoBehaviour, UI_IAddressable
    {
        #region Fields & Properties

        [SerializeField]
        private string addressableName;
        
        [SerializeField]
        private TextMeshProUGUI lvlText;
        
        [SerializeField]
        private TextMeshProUGUI depthTxt;
        
        [SerializeField]
        private TextMeshProUGUI researchTxt;
        
        [SerializeField]
        private TextMeshProUGUI inputCountTxt;
        
        [SerializeField]
        private TextMeshProUGUI tickCountTxt;
        
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void CloseUI()
        {
            // Debug.Log($"{gameObject.name}: UI 종료 시도, addressable 주소 : {addressableName}");
            UILoader.Instance.HideUI(addressableName);
        }

        public void UpdateTest() {
            Debug.Log("Updated UI 테스트 호출");
            UpdateUITxt();
        }

        public void LinkAddressable(string address)
        {
            // Debug.Log($"{gameObject.name}: addressable 주소 설정 : {address}");
            addressableName = address;
        }
        #endregion

        #region private methods
        
        private void UpdateUITxt() {
            // lvlText.text = "77"; //TODO
            // depthTxt.text = "777"; //TODO
            // researchTxt.text = "77%"; //TODO
            // inputCountTxt.text += "7"; //TODO
            // tickCountTxt.text += "7"; //TODO
        }
        #endregion

        #region Unity event methods

        private void Update() {
        }
        #endregion


    }
}
