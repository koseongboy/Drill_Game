using UnityEngine;
using UnityEngine.UIElements;

using DrillGame.UI.Interface;

namespace DrillGame.UI
{
    public class UI_Facility_Core : MonoBehaviour, UI_IAddressable
    {
        #region Fields & Properties
        [SerializeField]
        private Button btn_Close;

        [SerializeField]
        private string addressableName;
        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void CloseUI()
        {
            Debug.Log($"{gameObject.name}: UI 종료 시도, addressable 주소 : {addressableName}");
            UILoader.Instance.HideUI(addressableName);
        }

        public void LinkAddressable(string address)
        {
            Debug.Log($"{gameObject.name}: addressable 주소 설정 : {address}");
            addressableName = address;
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        private void Awake()
        {
            Debug.Log($"{gameObject.name}: Awake 호출");
        }
        #endregion


    }
}
