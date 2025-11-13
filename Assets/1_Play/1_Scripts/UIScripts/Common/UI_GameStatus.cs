using System;
using DrillGame.UI.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace DrillGame
{
    public class UI_GameStatus : MonoBehaviour, UI_IAddressable
    {
        #region Fields & Properties

        [SerializeField]
        private string addressableName;
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void CloseUI()
        {
            // 여기서 UI 닫힐 때 연출.
            UI.UILoader.Instance.HideUI(addressableName);
        }

        public void LinkAddressable(string address)
        {
            addressableName = address;
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods

        #endregion
    }
}
