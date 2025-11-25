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
    }
    #endregion

    #region getters & setters
    #endregion

    #region public methods
    public override void Run(int intensity) // todo: 레벨에 따른 연구 진척도 증가량 조절
    {
      for (int i = 0; i < intensity; i++)
      {
        ResearchManager.Instance.AddResearchProgress();
      }
    }
    #endregion

    #region private methods
    #endregion

    #region Unity event methods
    #endregion
    
  }
}