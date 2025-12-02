using UnityEngine;
using DrillGame.Managers;

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
      base.Run(intensity);

      for (int i = 0; i < intensity; i++)
      {
        EngineMergerManager.Instance.RunEngineMergeProcess();
      }
      
    }

    public void UpgradeLevel()
    {
      itemId += 1;
      facilityId += 1;
    }
    #endregion

    #region private methods
    #endregion

    #region Unity event methods
    #endregion

  }
}