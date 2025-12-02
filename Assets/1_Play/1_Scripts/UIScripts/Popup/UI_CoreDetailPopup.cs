using DG.Tweening;
using DrillGame.UI;
using UnityEngine;
using UnityEngine.UIElements;

using DrillGame.UI.Interface;

namespace DrillGame
{
    public class UI_CoreDetailPopup : UITemplate_DetailPopup
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
            CloseAction();
        }

        public void OpenFacilityCraft()
        {
            UILoader.Instance.ShowUI("UI_FacilityBuild");
        }

        public void TryCoreUpgrade()
        {
            if (CoreManager.Instance.TryCoreUpgrade())
            {
                CloseUI();
            }
        }
        #endregion

        #region private methods
        protected override void UpdateDetail()
        {
            var coreLevel = CoreManager.Instance.GetCoreLevel();

            titleTxt.text = $"코어 Lv.{coreLevel}";
            descTxt.text = "공장의 모든 엔진을 가동하기 위한 코어이다.\n이곳에서 시설을 새로 지을 수 있다.";
            // Sprite
            Sprite icon = Resources.Load<Sprite>("Icon/ItemIcon/" + "coreIcon");
            if (icon == null)
            {
                Debug.LogError($"Error: Resources 폴더에서 스프라이트 자원을 찾을 수 없습니다. : coreIcon");
            }
            iconImg.sprite = icon;
        }
        #endregion
        
        #region Unity Event

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateDetail();
        }
        

        #endregion
    }
}
