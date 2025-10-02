using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

using DrillGame.Components.Facility;
using DrillGame.Managers;

namespace DrillGame.Entity.Facility
{
    public abstract class Facility_Base : Entity_Base
    {
        #region Fields & Properties

        

        private FacilityController facilityController;

        #endregion

        #region initialization
        public Facility_Base(FacilityController facilityController, Vector2Int position)
        {
            Initialize(facilityController, position);
        }

        private void Initialize(FacilityController facilityController, Vector2Int position)
        {
            entityName = GetType().Name;
            BoardManager.Instance.RegisterFacility(this);

            Debug.Log($"{entityName} 생성 및 BoardManager register.");

            this.facilityController = facilityController;
            this.position = position;
        }
        #endregion

        #region getters & setters
        public List<Vector2Int> GetAllPositions()
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            foreach (var offset in TileFormation)
            {
                positions.Add(position + offset);
            }
            return positions;
        }
        public string GetFacilityName()
        {
            return entityName;
        }

        public string GetFacilityUIName()
        {
            return "UI_" + entityName;
        }
        #endregion

        #region public methods


        public virtual void ActivateFacility()
        {
            Debug.Log($"{entityName} 시설이 활성화되었습니다.");
            facilityController.UpDateFacilityObject();
        }
        #endregion

        #region private methods
        #endregion


    }
}
