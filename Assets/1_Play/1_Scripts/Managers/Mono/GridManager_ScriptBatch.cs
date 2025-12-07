using DrillGame.Managers;
using DrillGame.View.Helper;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DrillGame.Managers
{
    public partial class GridManager : MonoBehaviour
    {
        public void EntityBatch(List<BatchData> batchDatas)
        {
            foreach (BatchData data in batchDatas)
            {
                int entityId = ScriptableObjectManager.Instance.GetData<Item_Data_>(data.EntityItemID).EntityId;
                
                bool isFacility = entityId < 200000;

                Batch(data.batchPosition, data.EntityItemID, entityId, isFacility);
            }
        }


        private void Batch(Vector2Int pos, int itemId, int entityId, bool isFacility)
        {
            // update Action 전환

            Vector2Int cellPos2D = pos;
            Vector3Int cellPosition = new Vector3Int(cellPos2D.x, cellPos2D.y, 0);
            HashSet<Vector2Int> occupiedPositions = isFacility ? FacilityOccupiedPositions : EngineOccupiedPositions;
            occupiedPositions.Add(cellPos2D);

            EnterBatchMode(isFacility ? TilemapType.Facility : TilemapType.Engine, entityId);

            // Instantiate entity
            GameObject gameObject = Instantiate(entityObject, grid.CellToWorld(cellPosition) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, entityParent);
            if (gameObject.TryGetComponent<IDrillGameObjectInit>(out var init))
            {
                init.Initialize(cellPos2D, itemId, entityId); // Updated to use id instead of level
                // set sorting layer in parent
                gameObject.GetComponent<SpriteRenderer>().sortingLayerID = entityParent.GetComponent<Tilemap>().GetComponent<TilemapRenderer>().sortingLayerID;
            }
            else
            {
                Debug.LogError("IDrillGameObjectInit 인터페이스를 구현하지 않은 오브젝트입니다.");
            }

            ExitBatchMode();
        }
    }


    [System.Serializable]
    public class BatchData
    {
        public int EntityItemID;
        public Vector2Int batchPosition;
    }
}
