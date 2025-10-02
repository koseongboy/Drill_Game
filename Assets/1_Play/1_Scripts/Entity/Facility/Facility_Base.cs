using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

using DrillGame.Managers;
using DrillGame.Components;
using DrillGame.Components.Facility;

namespace DrillGame.Entity.Facility
{
    public abstract class Facility_Base : Entity_Base
    {
        #region Fields & Properties

        

        

        #endregion

        #region initialization
        public Facility_Base(ComponentBaseController baseController, Vector2Int position) : base(baseController, position)
        {

        }

        protected override void Initialize(ComponentBaseController baseController, Vector2Int position)
        {
            base.Initialize(baseController, position);
            BoardManager.Instance.RegisterFacility(this);

            Debug.Log($"{entityName} 시설 생성 및 BoardManager register.");
        }
        #endregion

        #region getters & setters
        

        public string GetFacilityUIName()
        {
            return "UI_" + entityName;
        }
        #endregion

        #region public methods


        public virtual void ActivateFacility()
        {
            Debug.Log($"{entityName} 시설이 활성화되었습니다.");
            FacilityController facilityController = baseController as FacilityController;
            facilityController.UpDateFacilityObject();
        }
        #endregion

        #region private methods
        #endregion


    }
}
