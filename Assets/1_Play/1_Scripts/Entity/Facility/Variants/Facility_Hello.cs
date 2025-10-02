using DrillGame.Components.Facility;
using UnityEngine;

namespace DrillGame.Entity.Facility
{
    public class Facility_Hello : Facility_Base
    {
        #region Fields & Properties


        #endregion

        #region Singleton & initialization
        public Facility_Hello(Vector2Int position) : base(position)
        {
            Debug.Log("Hello 시설이 생성되었습니다.");
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public override void ActivateFacility()
        {
            base.ActivateFacility();
            Debug.Log("Hello 시설이 활성화 : 안녕 세상아!");
        }
        #endregion

        #region private methods
        #endregion

    }
}
