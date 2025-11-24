using System;
using DrillGame.Core.Ground;
using DrillGame.UI.Interface;
using DrillGame.View.Ground;
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
        
        [SerializeField]
        private TextMeshProUGUI drillDamageText;
        
        [SerializeField]
        private TextMeshProUGUI groundHpText;
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

        private void OnChangeDrillDamage( int damage )
        {
            drillDamageText.text = damage.ToString();
        }

        private void OnChangeGroundHp( int hp )
        {
            groundHpText.text = hp.ToString();
        }
        
        #endregion

        #region Unity event methods

        private void OnEnable()
        {
            ES3File es3File = new ES3File("GroundUserData.es3");
            OnChangeGroundHp( es3File.Load<int>("GroundHP") );
            GroundComponent.Instance.OnHpChanged += OnChangeGroundHp;
        }

        private void OnDisable()
        {
            GroundComponent.Instance.OnHpChanged -= OnChangeGroundHp;
        }

        #endregion
    }
}
