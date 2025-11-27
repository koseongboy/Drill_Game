using UnityEngine;

namespace DrillGame.Core.Facility
{
  public class ResourceMergerEntity : FacilityEntity
  {
    #region Fields & Properties
    #endregion

    #region Singleton & initialization
    public ResourceMergerEntity(Vector2Int startPosition, int level, int itemId, int entityId) : base(startPosition, level, itemId, entityId)
    {
      Debug.Log("자원합병시설 생성됨.");
    }
    #endregion

    #region getters & setters
    #endregion

    #region public methods
    public override void Run(int intensity)
    {
      base.Run(intensity);  //todo: 로직 구현 해야함.
    }
    #endregion

    #region private methods
    #endregion

    #region Unity event methods
    #endregion
    
  }
}