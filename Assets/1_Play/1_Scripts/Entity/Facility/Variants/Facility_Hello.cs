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
            Debug.Log("Hello �ü��� �����Ǿ����ϴ�.");
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public override void ActivateFacility()
        {
            base.ActivateFacility();
            Debug.Log("Hello �ü��� Ȱ��ȭ : �ȳ� �����!");
        }
        #endregion

        #region private methods
        #endregion

    }
}
