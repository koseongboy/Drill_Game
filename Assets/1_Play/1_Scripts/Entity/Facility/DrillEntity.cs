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
    #endregion

    #region getters & setters
    #endregion

    #region public methods
    #endregion

    #region private methods
    #endregion

    #region Unity event methods
    #endregion
    public DrillEntity(Vector2Int startPosition, List<Vector2Int> formations, int level) : base(startPosition, formations, level)
    {
    }

    public new void Run(int intensity)
    {
        for (int i = 0; i < intensity; i++)
            {
                GroundComponent.Instance.GiveDamage(data.Level);
                Debug.Log("땅에 " + data.Level + " 만큼 데미지를 줌 (남은 땅의 체력: " + GroundComponent.Instance.GroundEntity.CurrentHp + ")");
            }
    }
  }
}