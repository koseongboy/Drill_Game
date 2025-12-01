using System;
using DrillGame.Core.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DrillGame.Managers
{
    public class EngineMergerData
    {
        public EngineMergerManager.MergeProcessingType type;
        
        public int progress;
        
        public int targetEngineItemId;
        public int inputEngineItemId1;
        public int inputEngineItemId2;

        public EngineMergerData(EngineMergerManager.MergeProcessingType _type,
            int _progress, int _targetEngineItemId = 0, int _inputEngineItemId1 = 0, int _inputEngineItemId2 = 0)
        {
            type = _type;
            progress = _progress;
            targetEngineItemId = _targetEngineItemId;
            inputEngineItemId1 = _inputEngineItemId1;
            inputEngineItemId2 = _inputEngineItemId2;
        }
    }
    
    public class EngineMergerManager : MonoBehaviour
    {
        #region Singleton & initialization
        public static EngineMergerManager Instance { get; private set; }
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
            
            LoadEngineMergerDataFromES3();
        }
        #endregion

        #region Fields & Properties
        public enum MergeProcessingType
        {
            Create = 1,
            Combine = 2,
            None = 0
        }

        private MergeProcessingType currentType = MergeProcessingType.None;
        
        [SerializeField] private int progress;
        [SerializeField] private int progressMax = 5;
        [SerializeField] private int progressDelta = 1;
        
        private int targetEngineItemId;
        private int inputEngineItemId1;
        private int inputEngineItemId2;

        public Action OnProcessChanged;
        
        private const string ENGINE_MERGER_KEY = "EngineMergerData";
        #endregion

        #region getters & setters

        public int GetProgress()
        {
            return progress;
        }

        public MergeProcessingType GetCurrentType()
        {
            return currentType;
        }

        public int GetTargetEngineItemId()
        {
            return targetEngineItemId;
        }
        #endregion

        #region public methods
        // Facility에서 호출될 함수
        // Facility에서 호출될 함수
        // Facility에서 호출될 함수
        public void RunEngineMergeProcess()
        {
            Debug.Log("엔진 합성기 작동 호출됨.");
            if (currentType == MergeProcessingType.None)
            {
                Debug.LogWarning("지금 엔진 합성기에 아무 작업도 할당되어있지 않음. 리턴.");
                return;
            }
            
            progress += progressDelta;
            if (progress >= progressMax)
            {
                progress = progressMax;
                OnCompleteProcess();
            }
        }
        
        public void StopEngineMergeProcess()
        {
            if (currentType == MergeProcessingType.Create)
            {
                targetEngineItemId = 0;
            }else if (currentType == MergeProcessingType.Combine)
            {
                RollbackInputEngineItems();
                
                targetEngineItemId = 0;
                inputEngineItemId1 = 0;
                inputEngineItemId2 = 0;
            }

            progress = 0;
            currentType = MergeProcessingType.None;
            OnProcessChanged?.Invoke();
        }

        // 엔진 생산 : 랜덤한 엔진을 뽑기
        public void RegisterEngineToCreate(int level)
        {
            currentType = MergeProcessingType.Create;
            var engineType = Random.Range(0, 5);
            targetEngineItemId = 1100 + engineType*10 + level;
            OnProcessChanged?.Invoke();
        }

        // 엔진 조합 : 두 엔진을 합쳐서 높은 레벨의 엔진 제작
        public void RegisterEngineToCombine(int targetEngineItemId, int inputEngineItemId1, int inputEngineItemId2)
        {
            currentType = MergeProcessingType.Combine;
            
            if (!InventoryManager.Instance.TryRemoveItem(inputEngineItemId1, 1))
            {
                Debug.LogWarning("인벤토리에 없는 아이템을 엔진합성기에 넣으려 합니다.");
                return;
            }
            if (!InventoryManager.Instance.TryRemoveItem(inputEngineItemId2, 1))
            {
                Debug.LogWarning("인벤토리에 없는 아이템을 엔진합성기에 넣으려 합니다.");
                InventoryManager.Instance.AddItem(inputEngineItemId1, 1);
                return;
            }
            
            this.targetEngineItemId = targetEngineItemId;
            this.inputEngineItemId1 = inputEngineItemId1;
            this.inputEngineItemId2 = inputEngineItemId2;
            
            OnProcessChanged?.Invoke();
        }

        public void RollbackInputEngineItems()
        {
            InventoryManager.Instance.AddItem(inputEngineItemId1, 1);
            InventoryManager.Instance.AddItem(inputEngineItemId2, 1);
            Init();
        }
        #endregion

        #region private methods

        private void Init()
        {
            currentType = MergeProcessingType.None;
            progress = 0;
            targetEngineItemId = 0;
            inputEngineItemId1 = 0;
            inputEngineItemId2 = 0;
        }
        
        private void OnCompleteProcess()
        {
            Debug.Log($"엔진 합성 끝! : {targetEngineItemId}");
            InventoryManager.Instance.AddItem(targetEngineItemId, 1);
            Init();
        }
        
        private void LoadEngineMergerDataFromES3()
        {
            var data = ES3.Load(ENGINE_MERGER_KEY, new EngineMergerData(0,0,0,0, 0));
            currentType = data.type;
            progress = data.progress;
            targetEngineItemId = data.targetEngineItemId;
            inputEngineItemId1 = data.inputEngineItemId1;
            inputEngineItemId2 = data.inputEngineItemId2;
        }
        
        private void SaveEngineMergerData()
        {
            var data = new EngineMergerData( currentType, progress, targetEngineItemId, inputEngineItemId1, inputEngineItemId2 );
            ES3.Save(ENGINE_MERGER_KEY, data);
        }
        #endregion

        #region Unity event methods
        private void OnApplicationQuit()
        {
            SaveEngineMergerData();
        }
        #endregion

        #region DEV

        [ContextMenu("현재 작업 완료시키기")]
        public void CompleteCurrentProcess()
        {
            progress = progressMax;
            OnCompleteProcess();
        }

        #endregion
    }
}