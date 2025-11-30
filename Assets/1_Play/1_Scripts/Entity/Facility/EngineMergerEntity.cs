using UnityEngine;
using DrillGame._1_Play._1_Scripts.Managers.Mono;

namespace DrillGame.Core.Facility
{
  public class EngineMergerEntity : FacilityEntity
  {
    #region Fields & Properties
    #endregion

    #region Singleton & initialization
    public EngineMergerEntity(Vector2Int startPosition, int level, int itemId, int entityId) : base(startPosition, level, itemId, entityId)
    {
    }
    #endregion

    #region getters & setters
    #endregion

    #region public methods
    public override void Run(int intensity)
    {
      for (int i = 0; i < intensity; i++)
      {
        EngineMergerManager.Instance.RunEngineMergeProcess();
      }
      
    }
    #endregion

    #region private methods
    #endregion

    #region Unity event methods
    #endregion

  }
}