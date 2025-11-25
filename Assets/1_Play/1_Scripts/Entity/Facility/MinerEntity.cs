using UnityEngine;

namespace DrillGame.Core.Facility
{
  public class MinerEntity : FacilityEntity
  {
    #region Fields & Properties
    #endregion

    #region Singleton & initialization
    public MinerEntity(Vector2Int startPosition, int id) : base(startPosition, id)
    {
        Debug.Log("채굴시설 생성됨.");
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