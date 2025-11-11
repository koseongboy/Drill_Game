using DrillGame.UI;
using DrillGame.UI.Interface;
using UnityEngine;
using UnityEngine.UIElements;

namespace DrillGame
{
    public class UI_UpperLowerToggle : MonoBehaviour, UI_IAddressable
    {
        #region Fields & Properties

        [SerializeField]
        private string addressableName;
        #endregion

        #region getters & setters
        #endregion

        #region public methods

        public void ButtonPressed() {
            Debug.Log("상부 하부 Toggle 눌림.");
        }
        
        public void CloseUI()
        {
            // Debug.Log($"{gameObject.name}: UI 종료 시도, addressable 주소 : {addressableName}");
            UILoader.Instance.HideUI(addressableName);
        }

        public void LinkAddressable(string address)
        {
            // Debug.Log($"{gameObject.name}: addressable 주소 설정 : {address}");
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

        private void OnEnable()
        {
            Debug.Log("UI_Facility_Core: OnEnable");
            OpenAction();
        }

        private void OpenAction()
        {
            // 여기서 UI 열릴 때 연출.
        }
    }
}
