using UnityEngine;

using DrillGame.View.Facility;
using DrillGame.Core.Facility;

namespace DrillGame.Core.Presenter
{
    public class FacilityPresenter
    {
        #region Fields & Properties
        private readonly FacilityComponent facilityComponent;
        private readonly FacilityEntity facilityEntity;


        #endregion

        #region Singleton & initialization

        public FacilityPresenter(FacilityComponent facilityComponent, FacilityEntity facilityEntity)
        {
            this.facilityComponent = facilityComponent;
            this.facilityEntity = facilityEntity;

            facilityEntity.OnFacilityActivated += OnFacilityEntityActivated;
            facilityEntity.OnFacilityDeleted += OnFacilityEntityDeleted;
        }

        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void Dispose()
        {
            facilityEntity.OnFacilityActivated -= OnFacilityEntityActivated;
            facilityEntity.OnFacilityDeleted -= OnFacilityEntityDeleted;
        }
        public void RequestFacilityDetail()
        {
            facilityEntity.ShowFacilityInfo();
        }
        #endregion

        #region private methods
        private void OnFacilityEntityActivated(int intensity)
        {
            facilityComponent.RunFacilityComponent(intensity);
        }

        private void OnFacilityEntityDeleted()
        {
            facilityComponent.DeleteFacilityComponent();
        }
        #endregion

        #region Unity event methods
        #endregion
    }
}
