using DrillGame.Managers;
using DrillGame.View.Helper;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DrillGame.Managers
{
    public partial class GridManager : MonoBehaviour
    {
        [SerializeField]
        private BatchData[] facilityBatchDatas;
        [SerializeField]
        private BatchData[] engineBatchDatas;
        public void FacilityBatch()
        {
            foreach (BatchData data in facilityBatchDatas)
            {
                Batch(data.batchPosition, data.EntityID, true);
            }
        }

        public void EngineBatch()
        {
            foreach (BatchData data in engineBatchDatas)
            {
                Batch(data.batchPosition, data.EntityID, false);
            }
        }

        private void Batch(Vector2Int pos, int id, bool isFacility)
        {
            // update Action 전환

            Vector2Int cellPos2D = pos;
            Vector3Int cellPosition = new Vector3Int(cellPos2D.x, cellPos2D.y, 0);
            HashSet<Vector2Int> occupiedPositions = isFacility ? FacilityOccupiedPositions : EngineOccupiedPositions;
            occupiedPositions.Add(cellPos2D);

            EnterBatchMode(isFacility ? TilemapType.Facility : TilemapType.Engine, id);

            // Instantiate entity
            GameObject gameObject = Instantiate(entityObject, grid.CellToWorld(cellPosition) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, entityParent);
            if (gameObject.TryGetComponent<IDrillGameObjectInit>(out var init))
            {
                init.Initialize(cellPos2D, 0, id); // Updated to use id instead of level
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
        public int EntityID;
        public Vector2Int batchPosition;
    }
}
