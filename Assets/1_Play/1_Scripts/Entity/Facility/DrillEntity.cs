using System.Collections;
using System.Collections.Generic;
using DrillGame.View.Ground;
using UnityEngine;
using DrillGame.View.Drill;

namespace DrillGame.Core.Facility
{
  public class DrillEntity : FacilityEntity
  {
    #region Fields & Properties
    DrillComponent dc;
    #endregion

    #region Singleton & initialization
    public DrillEntity(Vector2Int startPosition, int level, int itemId = 1, int entityId = 112001) : base(startPosition, level, itemId, entityId)
    {
      dc = DrillComponent.Instance;
      synergyText = "드릴 작동 시설은 위치가 고정임.";
      Debug.Log("드릴시설과 드릴 연동 완료");
    }
    #endregion

    #region getters & setters
    #endregion

    #region public methods
    public override void Run(int intensity, bool isSynergyed = false)
    {
      for (int i = 0; i < intensity; i++)
      {
          GroundComponent.Instance.GiveDamage(dc.GetDrillDamage());
      }
      base.Run(intensity);
      dc.RunDrillAnimation();

    }

    public void levlUp(int toWhat) //몇까지??
    {
      dc.levelUp(toWhat);
      facilityId = 11200 + toWhat;
      data = ScriptableObjectManager.Instance.GetData<Facility_Data_>(facilityId);
      this.Level = toWhat;
    }
    #endregion

    #region private methods
    
    #endregion

    #region Unity event methods
    #endregion
    

    
  }
}