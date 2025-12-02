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
      Debug.Log("엔진 합성기 생성됨.");
      foreach(var formation in GetFormationPositions())
      {
          if(formation.y >= -1 && formation.y <= 1) synergyCount++;
      }
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
        runCount++;
        if(runCount % formCount < synergyCount) {
          EngineMergerManager.Instance.RunEngineMergeProcess();
        }
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