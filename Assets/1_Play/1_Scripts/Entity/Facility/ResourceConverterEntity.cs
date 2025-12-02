using UnityEngine;
using DrillGame._1_Play._1_Scripts.Managers.Mono;
using DrillGame.Managers;

namespace DrillGame.Core.Facility
{
  public class ResourceConverterEntity : FacilityEntity
  {
    #region Fields & Properties
    #endregion

    #region Singleton & initialization
    public ResourceConverterEntity(Vector2Int startPosition, int level, int itemId, int entityId) : base(startPosition, level, itemId, entityId)
    {
      Debug.Log("자원합병시설 생성됨.");
      foreach(var formation in GetFormationPositions())
      {
          if(formation.y < -1) synergyCount++;
      }
      
      synergyText = $"자원 합병 시설은 코어 아래쪽에 배치하면 시너지를 받습니다. 시너지 활성화됨 ({synergyCount}/{formCount})";
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
          ResourceConverter.Instance.RunProcess();
        }
        ResourceConverter.Instance.RunProcess();
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