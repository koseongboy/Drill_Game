using UnityEngine;
using DrillGame.Core.Managers;

namespace DrillGame.Core.Facility
{
  public class MinerEntity : FacilityEntity
  {
    #region Fields & Properties
    #endregion

    #region Singleton & initialization
    public MinerEntity(Vector2Int startPosition, int level, int itemId = 1, int entityId = 101011) : base(startPosition, level, itemId, entityId)
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

      for (int i = 0; i < intensity; i++)
      {
        InventoryManager.Instance.AddItem( data.OutputItemId ); //이렇게 인벤에 추가하는거 맞는 지 확인
      }
    }
    #endregion

    #region private methods
    #endregion

    #region Unity event methods
    #endregion
    
  }
}