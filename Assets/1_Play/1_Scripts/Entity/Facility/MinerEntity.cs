using UnityEngine;
using DrillGame.Core.Managers;
using DrillGame.UI;
using DrillGame.View.Drill;
using DrillGame.View.Ground;

namespace DrillGame.Core.Facility
{
  public class MinerEntity : FacilityEntity
  {
    #region Fields & Properties
    GroundComponent gc;
    #endregion

    #region Singleton & initialization
    public MinerEntity(Vector2Int startPosition, int level, int itemId = 1, int entityId = 101011) : base(startPosition, level, itemId, entityId)
    {
        Debug.Log("채굴시설 생성됨.");
        gc = GroundComponent.Instance;
        foreach(var formation in GetFormationPositions())
        {
            if(formation.x == -4 || formation.x == 2) synergyCount++;
        }
        synergyText = $"채굴 시설은 좌우 가장자리에 놓으면 시너지를 받습니다. 시너지 활성화됨 ({synergyCount}/{formCount})";
    }
    #endregion

    #region getters & setters
    #endregion

    #region public methods
    public override void Run(int intensity, bool isSynergyed = false)
    {
      
      if (!gc.CanGetDropItem( data.OutputItemId ))
      {
        var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(data.OutputItemId);
        UILoader.Instance.ShowAlert($"현재 땅에서 채굴할 수 없는 자원입니다.\n자원 : {itemData.DisplayName}");
        Debug.Log($"현재 땅에서 채굴할 수 없는 자원입니다.\n자원 : {itemData.DisplayName}");
      } else
      {
        for (int i = 0; i < intensity; i++)
        {
          runCount++;
          isSynergyed = runCount % formCount < synergyCount;
          if(isSynergyed) {
            InventoryManager.Instance.AddItem( data.OutputItemId, data.OutputItemCount );
          }
          InventoryManager.Instance.AddItem( data.OutputItemId, data.OutputItemCount ); //이렇게 인벤에 추가하는거 맞는 지 확인
        }
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