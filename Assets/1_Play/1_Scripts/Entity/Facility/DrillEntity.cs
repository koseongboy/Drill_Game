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
      Debug.Log("드릴시설과 드릴 연동 완료");
    }
    #endregion

    #region getters & setters
    #endregion

    #region public methods
    public override void Run(int intensity)
    {
            base.Run(intensity);

            for (int i = 0; i < intensity; i++)
            {
                GroundComponent.Instance.GiveDamage(dc.GetDrillDamage());
            }
    }
    #endregion

    #region private methods
    #endregion

    #region Unity event methods
    #endregion
    

    
  }
}