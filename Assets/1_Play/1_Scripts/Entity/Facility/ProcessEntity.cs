using UnityEngine;

namespace DrillGame.Core.Facility
{
  public class ProcessEntity : FacilityEntity
  {
    #region Fields & Properties
    #endregion

    #region Singleton & initialization
    public ProcessEntity(Vector2Int startPosition, int level, int itemId = 1, int entityId = 101021) : base(startPosition, level, itemId, entityId)
    {
        Debug.Log("가공시설 생성됨.");
    }
    #endregion

    #region getters & setters
    #endregion

    #region public methods
    public override void Run(int intensity)
    {
      base.Run(intensity);
    }
    #endregion

    #region private methods
    #endregion

    #region Unity event methods
    #endregion
    
  }
}