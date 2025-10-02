using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

using DrillGame.Managers;
using DrillGame.Components;
using DrillGame.Components.Facility;
using System;

namespace DrillGame.Entity.Facility
{
    public abstract class Facility_Base : Entity_Base
    {
        #region Fields & Properties

        public event Action OnActivated;
        public event Action OnUpdated;


        #endregion

        #region initialization
        public Facility_Base(Vector2Int position) : base(position)
        {

        }

        protected override void Initialize(Vector2Int position)
        {
            base.Initialize(position);
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

            
            OnActivated?.Invoke();
        }
        #endregion

        #region private methods
        #endregion


    }
}
