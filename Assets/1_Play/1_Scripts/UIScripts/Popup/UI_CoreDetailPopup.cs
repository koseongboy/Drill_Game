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
        #endregion

        #region getters & setters
        #endregion

        #region public methods

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
        #endregion
        
        #region Unity Event

        protected override void OnEnable()
        {
            base.OnEnable();
            
            var coreLevel = CoreManager.Instance.GetCoreLevel();
            UpdateDetail($"코어 Lv.{coreLevel}",
                "공장의 모든 엔진을 가동하기 위한 코어이다. 이곳에서 시설을 새로 지을 수 있다.",
                "coreIcon");
        }
        

        #endregion
    }
}
