using DrillGame.Core.Facility;
using DrillGame.UI;
using UnityEngine;

namespace DrillGame
{
    public class UI_LabDetailPopup : UITemplate_DetailPopup
    {
        #region Fields & Properties
        private FacilityEntity labFacilityEntity;
        #endregion
        
        #region getters & setters
        public override void SetData(object entity)
        {
            Debug.Log(entity.ToString());
            labFacilityEntity = (FacilityEntity)entity;
            if (labFacilityEntity.synergyCount > 0)
            {
                SetIsOnSynergy(true);
            }
            else
            {
                SetIsOnSynergy(false);
            }

            var data = ScriptableObjectManager.Instance.GetData<Facility_Data_>(labFacilityEntity.GetFacilityId());
            UpdateDetail(data.DisplayName, data.GetFacilityDesc() + "\n"+labFacilityEntity.synergyText, data.Icon);
        }
        #endregion

        #region public methods
        public override void MoveOnBoard()
        {
            CloseUI();
            labFacilityEntity.MoveEntity(); 
        }
        
        public override void DeleteOnBoard()
        {
            CloseUI();
            labFacilityEntity.DeleteEntity();
        }

        public void OpenLabUI()
        {
            UILoader.Instance.ShowUI("UI_Research");
        }
        #endregion

        #region private methods
        #endregion
    }
}
