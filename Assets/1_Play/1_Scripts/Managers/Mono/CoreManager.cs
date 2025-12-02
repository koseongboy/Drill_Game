using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using DrillGame.Core.Managers;
using DrillGame.UI;
using DrillGame.View.Ground;

namespace DrillGame
{
    public class CoreManager : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField] private int coreLevel;
        
        private Vector2Int CORE_POSITION = new Vector2Int(-1, 0);
        private List<Vector2Int> CORE_FORMATIONS = new List<Vector2Int>()
        {
            // 3*3   -  와우; 미친 상남자의 하드코딩 - 눈치...
            new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1),
            new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0),
            new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
        };

        private Core_Data_ core_Data;
        private const int CORE_DATA_START_ID = 2000;


        public event Action<int> OnCoreLevelChanged;

        #endregion

        #region Singleton & initialization
        public static CoreManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }

            Initialize();
        }

        private void Initialize()
        {
            int tempCoreDataId = CORE_DATA_START_ID + 1;
            core_Data = ScriptableObjectManager.Instance.GetData<Core_Data_>(tempCoreDataId);
            SetCoreLevel( core_Data.Level );
        }
        #endregion

        #region getters & setters

        private void SetCoreLevel(int level)
        {
            coreLevel = level;
            OnCoreLevelChanged?.Invoke(coreLevel);
        }

        public int GetCoreLevel()
        {
            return coreLevel;
        }

        public Vector2Int GetCorePosition()
        {
            return CORE_POSITION;
        }

        public Vector2 GetCoreWorldPosition()
        {
            return transform.position;
        }

        public List<Vector2Int> GetCoreFormations()
        {
            List<Vector2Int> absolutePositions = new List<Vector2Int>();
            foreach (var formation in CORE_FORMATIONS)
            {
                absolutePositions.Add(CORE_POSITION + formation);
            }
            return absolutePositions;
        }

        public int GetMaxFacilityCount()
        {
            return core_Data.FacilityCount;
        }

        public int GetMaxEngineCount()
        {
            return core_Data.EngineCount;
        }
        #endregion

        #region public methods
        public bool TryCoreUpgrade()
        {
            int nextCoreDataId = core_Data.GetId() + 1;
            Core_Data_ nextCoreData = ScriptableObjectManager.Instance.GetData<Core_Data_>(nextCoreDataId);
            if (nextCoreData == null)
            {
                UILoader.Instance.ShowAlert("최고 레벨 코어에 도달했습니다.");
                return false;
            }

            // 깊이 체크
            int requiredDepth = core_Data.UpgradeRequiredDepth;
            if(GroundComponent.Instance.GetDepth() < requiredDepth)
            {
                UILoader.Instance.ShowAlert("코어 업그레이드에 필요한 깊이에 도달하지 못했습니다.");
                return false;
            }



            // 자원 체크
            int requiredResourceId = core_Data.UpgradeRequiredItemId;
            int requiredResourceCount = core_Data.UpgradeRequiredItemCount;
            if (!InventoryManager.Instance.HasItem(requiredResourceId, requiredResourceCount))
            {
                var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(requiredResourceId);
                UILoader.Instance.ShowAlert($"코어 업그레이드에 필요한 자원이 부족합니다.\n필요 자원 : {itemData.DisplayName} {requiredResourceCount}개");
                return false;
            }

            // 자원 차감
            InventoryManager.Instance.TryRemoveItem(requiredResourceId, requiredResourceCount);

            // 업그레이드 적용
            core_Data = nextCoreData;
            SetCoreLevel( core_Data.Level );
            UILoader.Instance.ShowAlert($"코어 업그레이드가 완료되었습니다. 새로운 코어 레벨: {coreLevel}");
            return true;
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
        
        #region DEV

        [ContextMenu("코어 레벨 업")]
        public void CoreLevelUp()
        {
            SetCoreLevel(coreLevel + 1);
            Debug.Log($"코어 레벨업 : 현재 {coreLevel}");
        }
        #endregion
    }
}
