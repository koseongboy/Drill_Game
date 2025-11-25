using System.Collections.Generic;
using UnityEngine;
using DrillGame.View.Ground;

namespace DrillGame.Core.Facility
{
  public class DrillEntity : FacilityEntity
  {
    #region Fields & Properties
    #endregion

    #region Singleton & initialization
    public DrillEntity(Vector2Int startPosition, int level, int itemId = 1, int entityId = 112001) : base(startPosition, level, itemId, entityId)
    {
      Debug.Log("드릴 생성됨.");
    }
    #endregion

    #region getters & setters
    #endregion

    #region public methods
    public override void Run(int intensity)
    {
        for (int i = 0; i < intensity; i++)
            {
                GroundComponent.Instance.GiveDamage(data.Level);
            }
    }
    #endregion

    #region private methods
    #endregion

    #region Unity event methods
    #endregion
    

    
  }
}