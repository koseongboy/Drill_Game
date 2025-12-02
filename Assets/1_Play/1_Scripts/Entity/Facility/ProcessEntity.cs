using UnityEngine;
using DrillGame.Core.Managers;

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
        foreach(var formation in GetFormationPositions())
      {
          if(formation.y == -6) synergyCount++;
      }
      
      synergyText = $"자원 합병 시설은 공장 맨 아래에 배치하면 시너지를 얻습니다. 시너지 활성화됨 ({synergyCount}/{formCount})";
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
          InventoryManager.Instance.TryRemoveItem(data.InputItemId, data.InputItemCount);
          InventoryManager.Instance.AddItem(data.OutputItemId, data.OutputItemCount);
          Debug.Log($"{data.InputItemId}아이디를 가진 자원을 ${data.InputItemCount}개 잃고, ${data.OutputItemId}를 ${data.OutputItemCount}개 얻음.");
        }
        InventoryManager.Instance.TryRemoveItem(data.InputItemId, data.InputItemCount);
        InventoryManager.Instance.AddItem(data.OutputItemId, data.OutputItemCount);
        Debug.Log($"{data.InputItemId}아이디를 가진 자원을 ${data.InputItemCount}개 잃고, ${data.OutputItemId}를 ${data.OutputItemCount}개 얻음.");
      }
    }
    #endregion

    #region private methods
    #endregion

    #region Unity event methods
    #endregion
    
  }
}