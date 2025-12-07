using DrillGame.Core.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.Managers
{
    public class SaveManager : MonoBehaviour
    {
        #region Fields & Properties
        [SerializeField]
        private List<BatchData> initBatchData;

        [SerializeField]
        private List<Vector2Int> initInventoryData;

        public static SaveManager Instance { get; private set; }

        private const string IS_FIRST_LAUNCH_KEY = "IsFirstLaunch";

        public static event Action OnRequestAllDataSave;

        public Dictionary<string, string> SaveKeys = new Dictionary<string, string>()
        {
            { "RESEARCH_SELECTED_ID_KEY", "ResearchIdData" },
            { "RESEARCH_PROGRESS_KEY", "ResearchProgressData" },
            { "ENGINE_MERGER_KEY", "EngineMergerData" },
            { "RESOURCE_CONVERTER_KEY", "ResourceConverterData" },
            { "DRILL_DATA_KEY", "DrillData" },
            { "GROUND_DEPTH_KEY", "GroundDepthData" },
            { "GROUND_HP_KEY", "GroundHPData" },
            { "INVENTORY_KEY", "InventoryData" },
            { "PLAY_TIME_KEY", "DailyPlayTimeData" },
            { "INPUT_COUNT_KEY", "DailyInputCountData" },
            { "CORE_LEVEL_KEY", "CoreLevelData" },
            { "ENTITY_BATCH_KEY", "EntityBatchData" }

        };

        #endregion

        #region Singleton & initialization
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
            if(IsFirstLaunch() == false)
            {
                Debug.Log("첫 실행 감지됨. 첫 실행 데이터 설정 중...");
                SetFirstLaunchData();
            }
            else
            {
                Debug.Log("첫 실행 아님. 저장된 데이터 검사 중...");
                CheckAllData();
            }

            //InventoryManager.Instance.CallInventoryManager();
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void SaveResearchId(int selectedResearchId)
        {
            ES3.Save(SaveKeys["RESEARCH_SELECTED_ID_KEY"], selectedResearchId);
        }

        public int LoadResearchId(int defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["RESEARCH_SELECTED_ID_KEY"]))
            {
                return ES3.Load<int>(SaveKeys["RESEARCH_SELECTED_ID_KEY"]);
            }

            Debug.LogWarning("No saved ResearchId found.");
            return defaultValue;
        }

        public void DeleteResearchId()
        {
            ES3.DeleteKey(SaveKeys["RESEARCH_SELECTED_ID_KEY"]);
        }

        public void SaveResearchProgressData(Dictionary<int, float> researchProgresses)
        {
            ES3.Save(SaveKeys["RESEARCH_PROGRESS_KEY"], researchProgresses);
        }


        public Dictionary<int, float> LoadResearchProgressData(Dictionary<int, float> defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["RESEARCH_PROGRESS_KEY"]))
            {
                return ES3.Load<Dictionary<int, float>>(SaveKeys["RESEARCH_PROGRESS_KEY"]);
            }

            Debug.LogWarning("No saved ResearchProgressData found.");
            return defaultValue;
        }

        public void DeleteResearchProgressData()
        {
            ES3.DeleteKey(SaveKeys["RESEARCH_PROGRESS_KEY"]);
        }

        [ContextMenu("리서치 매니저 단독 초기화")]
        public void InitializeResearchManagerData()
        {
            ES3.Save(SaveKeys["RESEARCH_SELECTED_ID_KEY"], 1);
            ES3.Save(SaveKeys["RESEARCH_PROGRESS_KEY"], new Dictionary<int, float>());
            Debug.Log("리서치 매니저 데이터가 초기화되었습니다.");
        }


        public void SaveEngineMergerData(EngineMergerData engineMergerData)
        {
            ES3.Save(SaveKeys["ENGINE_MERGER_KEY"], engineMergerData);
        }
        public EngineMergerData LoadEngineMergerData(EngineMergerData defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["ENGINE_MERGER_KEY"]))
            {
                return ES3.Load<EngineMergerData>(SaveKeys["ENGINE_MERGER_KEY"]);
            }
            Debug.LogWarning("No saved EngineMergerData found.");
            return defaultValue;
        }
        public void DeleteEngineMergerData()
        {
            ES3.DeleteKey(SaveKeys["ENGINE_MERGER_KEY"]);
        }

        [ContextMenu("엔진 머저 매니저 단독 초기화")]
        public void InitializeEngineMergerManagerData()
        {
            var defaultData = new EngineMergerData(0, 0, 0, 0);
            ES3.Save(SaveKeys["ENGINE_MERGER_KEY"], defaultData);
            Debug.Log("엔진 머저 매니저 데이터가 초기화되었습니다.");
        }

        public void SaveResourceConverterData(ResourceConverter_SaveData data)
        {
            ES3.Save(SaveKeys["RESOURCE_CONVERTER_KEY"], data);
        }

        public ResourceConverter_SaveData LoadResourceConverterData(ResourceConverter_SaveData defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["RESOURCE_CONVERTER_KEY"]))
            {
                return ES3.Load<ResourceConverter_SaveData>(SaveKeys["RESOURCE_CONVERTER_KEY"]);
            }
            Debug.LogWarning("No saved ResourceConverterData found.");
            return defaultValue;
        }

        public void DeleteResourceConverterData()
        {
            ES3.DeleteKey(SaveKeys["RESOURCE_CONVERTER_KEY"]);
        }

        [ContextMenu("리소스 컨버터 매니저 단독 초기화")]
        public void InitializeResourceConverterManagerData()
        {
            var defaultData = new ResourceConverter_SaveData(0, 0);
            ES3.Save(SaveKeys["RESOURCE_CONVERTER_KEY"], defaultData);
            Debug.Log("리소스 컨버터 매니저 데이터가 초기화되었습니다.");
        }

        public void SaveDrillData(int drillLevel)
        {
            ES3.Save(SaveKeys["DRILL_DATA_KEY"], drillLevel);
        }

        public int LoadDrillData(int defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["DRILL_DATA_KEY"]))
            {
                return ES3.Load<int>(SaveKeys["DRILL_DATA_KEY"]);
            }
            Debug.LogWarning("No saved DrillData found.");
            return defaultValue;
        }
        public void DeleteDrillData()
        {
            ES3.DeleteKey(SaveKeys["DRILL_DATA_KEY"]);
        }
        [ContextMenu("드릴 매니저 단독 초기화")]
        public void InitializeDrillManagerData()
        {
            ES3.Save(SaveKeys["DRILL_DATA_KEY"], 1);
            Debug.Log("드릴 매니저 데이터가 초기화되었습니다.");
        }

        public void SaveGroundDepthData(int depth)
        {
            ES3.Save(SaveKeys["GROUND_DEPTH_KEY"], depth);
        }
        public int LoadGroundDepthData(int defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["GROUND_DEPTH_KEY"]))
            {
                return ES3.Load<int>(SaveKeys["GROUND_DEPTH_KEY"]);
            }
            Debug.LogWarning("No saved GroundDepthData found.");
            return defaultValue;
        }
        public void DeleteGroundDepthData()
        {
            ES3.DeleteKey(SaveKeys["GROUND_DEPTH_KEY"]);
        }

        [ContextMenu("지면 깊이 매니저 단독 초기화")]
        public void InitializeGroundDepthManagerData()
        {
            ES3.Save(SaveKeys["GROUND_DEPTH_KEY"], 0);
            Debug.Log("지면 깊이 매니저 데이터가 초기화되었습니다.");
        }

        public void SaveGroundHPData(int hp)
        {
            ES3.Save(SaveKeys["GROUND_HP_KEY"], hp);
        }
        public int LoadGroundHPData(int defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["GROUND_HP_KEY"]))
            {
                return ES3.Load<int>(SaveKeys["GROUND_HP_KEY"]);
            }
            Debug.LogWarning("No saved GroundHPData found.");
            return defaultValue;
        }
        public void DeleteGroundHPData()
        {
            ES3.DeleteKey(SaveKeys["GROUND_HP_KEY"]);
        }

        [ContextMenu("지면 HP 매니저 단독 초기화")]
        public void InitializeGroundHPManagerData()
        {
            ES3.Save(SaveKeys["GROUND_HP_KEY"], 100);
            Debug.Log("지면 HP 매니저 데이터가 초기화되었습니다.");
        }

        public void SaveInventoryData(Dictionary<int, int> inventoryData)
        {
            ES3.Save(SaveKeys["INVENTORY_KEY"], inventoryData);
        }

        public Dictionary<int, int> LoadInventoryData(Dictionary<int, int> defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["INVENTORY_KEY"]))
            {
                var data = ES3.Load<Dictionary<int, int>>(SaveKeys["INVENTORY_KEY"]);
                return data;
            }
            Debug.LogWarning("No saved InventoryData found.");
            return defaultValue;
        }
        public void DeleteInventoryData()
        {
            ES3.DeleteKey(SaveKeys["INVENTORY_KEY"]);
        }
        
        
        
        
        public void SaveInputCountData(int inputCount)
        {
            string todayKey = DateTime.Today.ToString("yyyy-MM-dd"); // '2025-12-07' 형식

            Dictionary<string, int> dailyData = LoadAllInputCountData(new Dictionary<string, int>());
            dailyData[todayKey] = inputCount;
            ES3.Save(SaveKeys["INPUT_COUNT_KEY"], dailyData);
        }
        
        public Dictionary<string, int> LoadAllInputCountData(Dictionary<string, int> defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["INPUT_COUNT_KEY"]))
            {
                return ES3.Load<Dictionary<string, int>>(SaveKeys["INPUT_COUNT_KEY"]);
            }
        
            Debug.LogWarning("No saved InputCount found.");
            return defaultValue;
        }
        
        public int LoadTodayInputCount()
        {
            string todayKey = DateTime.Today.ToString("yyyy-MM-dd");
            Dictionary<string, int> dailyData = LoadAllInputCountData(new Dictionary<string, int>());

            dailyData.TryGetValue(todayKey, out int todayTime);
            return todayTime; // 저장된 시간이 없으면 0.0f 반환
        }
        
        public void DeleteInputCountData()
        {
            ES3.DeleteKey(SaveKeys["INPUT_COUNT_KEY"]);
        }
        
        
        
        
        public void SaveDailyPlayTimeData(int sessionPlayTime)
        {
            string todayKey = DateTime.Today.ToString("yyyy-MM-dd"); // '2025-12-07' 형식

            Dictionary<string, int> dailyData = LoadAllDailyPlayTimeData(new Dictionary<string, int>());
            dailyData[todayKey] = sessionPlayTime;
            ES3.Save(SaveKeys["PLAY_TIME_KEY"], dailyData);
        }
        
        public Dictionary<string, int> LoadAllDailyPlayTimeData(Dictionary<string, int> defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["PLAY_TIME_KEY"]))
            {
                return ES3.Load<Dictionary<string, int>>(SaveKeys["PLAY_TIME_KEY"]);
            }
        
            Debug.LogWarning("No saved DailyPlayTimeData found.");
            return defaultValue;
        }
        
        public float LoadTodayPlayTime()
        {
            string todayKey = DateTime.Today.ToString("yyyy-MM-dd");
            Dictionary<string, int> dailyData = LoadAllDailyPlayTimeData(new Dictionary<string, int>());

            dailyData.TryGetValue(todayKey, out int todayTime);
            return todayTime; // 저장된 시간이 없으면 0.0f 반환
        }
        
        public void DeleteDailyPlayTimeData()
        {
            ES3.DeleteKey(SaveKeys["PLAY_TIME_KEY"]);
        }
        
        
        

        public void SaveCoreLevelData(int coreLevel)
        {
            ES3.Save(SaveKeys["CORE_LEVEL_KEY"], coreLevel);
        }
        public int LoadCoreLevelData(int defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["CORE_LEVEL_KEY"]))
            {
                return ES3.Load<int>(SaveKeys["CORE_LEVEL_KEY"]);
            }
            Debug.LogWarning("No saved CoreLevelData found.");
            return defaultValue;
        }
        public void DeleteCoreLevelData()
        {
            ES3.DeleteKey(SaveKeys["CORE_LEVEL_KEY"]);
        }

        [ContextMenu("코어 레벨 매니저 단독 초기화")]
        public void InitializeCoreLevelManagerData()
        {
            ES3.Save(SaveKeys["CORE_LEVEL_KEY"], 1);
            Debug.Log("코어 레벨 매니저 데이터가 초기화되었습니다.");
        }

        public void SaveEntityBatchData(List<BatchData> data)
        {
            ES3.Save(SaveKeys["ENTITY_BATCH_KEY"], data);
        }
        public List<BatchData> LoadEntityBatchData(List<BatchData> defaultValue)
        {
            if (ES3.KeyExists(SaveKeys["ENTITY_BATCH_KEY"]))
            {
                return ES3.Load<List<BatchData>>(SaveKeys["ENTITY_BATCH_KEY"]);
            }
            Debug.LogWarning("No saved EntityBatchData found.");
            return defaultValue;
        }
        public void DeleteEntityBatchData()
        {
            ES3.DeleteKey(SaveKeys["ENTITY_BATCH_KEY"]);
        }
        [ContextMenu("엔티티 배치 매니저 단독 초기화")]
        public void InitializeEntityBatchManagerData()
        {
            ES3.Save(SaveKeys["ENTITY_BATCH_KEY"], new List<BatchData>());
            Debug.Log("엔티티 배치 매니저 데이터가 초기화되었습니다.");
        }




        [ContextMenu("리셋 데이터")]
        public void ResetAllData()
        {
            ES3.DeleteKey(IS_FIRST_LAUNCH_KEY);
            foreach (var key in SaveKeys.Values)
            {
                ES3.DeleteKey(key);
            }
            Debug.Log("모든 데이터가 리셋되었습니다.");
        }

        [ContextMenu("첫 실행 데이터 설정")]
        public void SetFirstLaunchData()
        {
            ES3.Save(IS_FIRST_LAUNCH_KEY, 1);
            ES3.Save(SaveKeys["RESEARCH_SELECTED_ID_KEY"], 1);
            ES3.Save(SaveKeys["RESEARCH_PROGRESS_KEY"], new Dictionary<int, float>());

            ES3.Save(SaveKeys["ENGINE_MERGER_KEY"], new EngineMergerData(0, 0, 0, 0));
            ES3.Save(SaveKeys["RESOURCE_CONVERTER_KEY"], new ResourceConverter_SaveData(0, 0));
            ES3.Save(SaveKeys["DRILL_DATA_KEY"], 1);
            ES3.Save(SaveKeys["GROUND_DEPTH_KEY"], 1);
            ES3.Save(SaveKeys["GROUND_HP_KEY"], ScriptableObjectManager.Instance.GetData<Ground_Data_>(5001).HP);

            Dictionary<int, int> initInventoryData = new Dictionary<int, int>();
            foreach (var item in this.initInventoryData)
            {
                initInventoryData[item.x] = item.y;
                Debug.Log($"인벤토리 초기 아이템 설정: ItemID {item.x}, Count {item.y}");
            }
            ES3.Save(SaveKeys["INVENTORY_KEY"], initInventoryData);

            ES3.Save(SaveKeys["PLAY_TIME_KEY"], new Dictionary<string, int>());
            ES3.Save(SaveKeys["CORE_LEVEL_KEY"], 1);
            ES3.Save(SaveKeys["ENTITY_BATCH_KEY"], initBatchData);

            Debug.Log("첫 실행 데이터가 설정되었습니다.");
        }

        public void RequestAllDataSave()
        {
            OnRequestAllDataSave?.Invoke();
            Debug.Log("모든 매니저에 데이터 저장 요청이 전송되었습니다.");
        }


        #endregion

        #region private methods
        private bool IsFirstLaunch()
        {
            if (ES3.KeyExists(IS_FIRST_LAUNCH_KEY))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckAllData()
        {
            foreach (var key in SaveKeys.Values)
            {
                if (!ES3.KeyExists(key))
                {
                    Debug.LogWarning($"No saved data found for key: {key}");
                }
            }
        }
        #endregion

        #region Unity event methods
        private void OnApplicationQuit()
        {
            RequestAllDataSave();
        }
        #endregion

        #region DEV

        [ContextMenu("Test playTime Data Save")]
        public void SavePlayTimeData()
        {
            
        }
        

        #endregion
    }
}
