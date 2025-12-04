using NUnit.Framework;
using UnityEngine;

namespace DrillGame.Core.Facility
{
  public class LabEntity : FacilityEntity
  {
    #region Fields & Properties
  
    #endregion

    #region Singleton & initialization
    public LabEntity(Vector2Int startPosition, int level, int itemId = 1, int entityId = 110001) : base(startPosition, level, itemId, entityId)
    {
      Debug.Log("연구소 생성됨.");
      foreach(var formation in GetFormationPositions())
      {
          if(formation.y < -1) synergyCount++;
      }
      
      synergyText = $"연구소는 코어 아래쪽에 배치하면 시너지를 받습니다. 시너지 활성화됨 ({synergyCount}/{formCount})";
      Debug.Log(synergyText);
    }
    #endregion

    #region getters & setters
    #endregion

    #region public methods
    public override void Run(int intensity, bool isSynergyed = false) // todo: 레벨에 따른 연구 진척도 증가량 조절
    {
      
      for (int i = 0; i < intensity; i++)
      {
        runCount++;
        isSynergyed = runCount % formCount < synergyCount;
        if(isSynergyed) {
          ResearchManager.Instance.AddResearchProgress();
        }
        ResearchManager.Instance.AddResearchProgress();
      }
      base.Run(intensity, isSynergyed);
    }
    #endregion

    #region private methods
    #endregion

    #region Unity event methods
    #endregion
    
  }
}