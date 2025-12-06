using System;
using System.Collections.Generic;
using System.Linq;
using DrillGame.Core.Managers;
using UnityEngine;
using MiniJSON;
using UnityEngine.InputSystem;
using DrillGame.Managers;
using DrillGame.UI;
using DrillGame.View.Drill;

namespace DrillGame
{
    public class ResearchManager : MonoBehaviour
    {
        [SerializeField]
        private int selectedResearchId = 0;
        [SerializeField]
        private float progressValue = 1f;
        
        private Dictionary<int, float> researchProgresses;
        
        public event Action<int, float, float> OnResearchProgressChanged;

        #region Singleton & initialization
        public static ResearchManager Instance { get; private set; }
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
        }
        #endregion

        #region Get & Set
        public float GetResearchProgress(int researchId)
        {
            return researchProgresses[researchId];
        }

        public float GetResearchProgressRate(int researchId) {
            var data = ScriptableObjectManager.Instance.GetData<Research_Data_>(researchId);
            var maxResearchAmount = data.ResearchAmount;
            return GetResearchProgress(researchId) / maxResearchAmount;
        }

        public int GetSelectedResearchId() {
            return selectedResearchId;
        }

        public bool IsResearchDone(int researchId) {
            return researchProgresses[researchId] >=
                   ScriptableObjectManager.Instance.GetData<Research_Data_>(researchId).ResearchAmount;
        }

        public bool IsResearchUnLocked(int researchId) {
            var data = ScriptableObjectManager.Instance.GetData<Research_Data_>(researchId);
            var requiredResearchId = data.RequireResearchId;
            return requiredResearchId == 0 || IsResearchDone(requiredResearchId);
        }

        #endregion
        
        #region public methods
        public void SelectResearch(int researchId)
        {
            selectedResearchId = researchId;
            SaveManager.Instance.SaveResearchId(selectedResearchId);
            OnResearchProgressChanged?.Invoke( selectedResearchId, researchProgresses[selectedResearchId],GetResearchProgressRate( selectedResearchId ) );
        }

        public void UnSelectResearch() {
            selectedResearchId = 0;
            SaveManager.Instance.SaveResearchId(selectedResearchId);
            OnResearchProgressChanged?.Invoke( 0, 0f, 0f);
        }
        
        [ContextMenu("Add Research Progress")]
        public void AddResearchProgress()
        {
            if (selectedResearchId == 0)
            {
                UILoader.Instance.ShowAlert("선택된 연구가 없습니다! 연구소가 놀고 있습니다!");
                return;
            }
            if (!researchProgresses.ContainsKey(selectedResearchId))
            {
                Debug.Log("올바르지 않은 ResearchKey입니다. : "+ selectedResearchId);
                return;
            }
            
            // Input 재료 부족하면 연구 진척 안 하기
            var researchData = ScriptableObjectManager.Instance.GetData<Research_Data_>(selectedResearchId);
            var inputItemId = researchData.InputItemPerTickId;
            var inputCount = researchData.InputItemPerTickCount;

            if (!InventoryManager.Instance.TryRemoveItem(inputItemId, inputCount))
            {
                UILoader.Instance.ShowAlert("자원이 부족해 연구를 진척시키지 못했습니다.");
                return;
            }
            
            researchProgresses[selectedResearchId] += progressValue;
            CheckCompleteResearch();
        }


        #endregion
        
        #region private method
        [ContextMenu("Initialize Progress Dict")]
        private void InitializeProgressDict()
        {
            var researchDatas = ScriptableObjectManager.Instance.GetAllData<Research_Data_>();
            researchProgresses = new Dictionary<int, float>();
            foreach (var researchData in researchDatas)
            {
                researchProgresses.Add(researchData.Key, 0f);
            }
            
            SaveManager.Instance.SaveResearchId(selectedResearchId);
            SaveManager.Instance.SaveResearchProgressData(researchProgresses);
            Debug.Log("InitializeProgressDict Done.");
        }
        
        private void LoadResearchKey()
        {
            selectedResearchId = SaveManager.Instance.LoadResearchId(1);
        }
        
        [ContextMenu("Load Progress Dict - ScriptableData 추가되면 실행")]
        private void LoadProgressDict()
        {
            researchProgresses = SaveManager.Instance.LoadResearchProgressData(null);

            if (researchProgresses == null)
            {
                InitializeProgressDict();
            }
            // legacy code

            //if (ES3.KeyExists(RESEARCH_PROGRESS_KEY))
            //{
            //    researchProgresses = ES3.Load(RESEARCH_PROGRESS_KEY, new Dictionary<int, float>());
            //}
            //else
            //{
            //    Debug.Log("[ResearchManager] Easy Save에 저장된 데이터가 없습니다. Dictionary를 새로 생성합니다.");
            //    InitializeProgressDict(); 
            //}
        }
        
        private void CheckCompleteResearch()
        {
            if (IsResearchDone(selectedResearchId)) {
                researchProgresses[selectedResearchId] = ScriptableObjectManager.Instance.GetData<Research_Data_>(selectedResearchId).ResearchAmount;

                if (selectedResearchId % 5 == 0) // 드릴 연구라면
                {
                    var level = (selectedResearchId-30000) / 5 + 1;
                    DrillComponent.Instance.levelUp( level );   
                }
                Debug.Log($"완료된 연구입니다. : {selectedResearchId}");
                UnSelectResearch();
            }
            OnResearchProgressChanged?.Invoke( selectedResearchId, researchProgresses[selectedResearchId], GetResearchProgressRate( selectedResearchId ) );
        }
        
        #endregion

        #region Unity event methods
        private void Start()
        {
            LoadResearchKey();
            LoadProgressDict();
            OnResearchProgressChanged?.Invoke( selectedResearchId, researchProgresses[selectedResearchId],GetResearchProgressRate( selectedResearchId ) );
        }
        
        private void OnApplicationQuit()
        {
            SaveManager.Instance.SaveResearchProgressData(researchProgresses);
        }
        #endregion

        #region DEV

        [ContextMenu("현재 연구 완료")]
        public void CompleteCurrentResearch()
        {
            if (!researchProgresses.ContainsKey(selectedResearchId))
            {
                Debug.Log("올바르지 않은 ResearchKey입니다. : "+ selectedResearchId);
                return;
            }
            
            researchProgresses[selectedResearchId] = ScriptableObjectManager.Instance.GetData<Research_Data_>(selectedResearchId).ResearchAmount;
            CheckCompleteResearch();
        }

        [ContextMenu("ES3 키 삭제")]
        public void DeleteResearchDataInES3()
        {
            SaveManager.Instance.DeleteResearchId();
            SaveManager.Instance.DeleteResearchProgressData();
        }
                
        /// <summary>
        /// 연구 진척도를 모두 0으로 바꾸는 함수!!! DEV용
        /// </summary>
        [ContextMenu("Test: 모든 연구 진척도 0으로 Reset")]
        public void ResetResearchProgress_Test()
        {
            List<int> keys = new List<int>(researchProgresses.Keys);

            foreach (int key in keys)
            {
                researchProgresses[key] = 0.0f;
            }
            Debug.Log("모든 연구 진척도가 0%로 초기화되었습니다.");
            OnResearchProgressChanged?.Invoke( selectedResearchId, researchProgresses[selectedResearchId],GetResearchProgressRate( selectedResearchId ) );
            SaveManager.Instance.SaveResearchProgressData(researchProgresses);
        }

        /// <summary>
        /// 연구 진척도 Dictionary 출력 함수. DEV용
        /// </summary>
        [ContextMenu("Test: 모든 연구 진척도 Print")]
        public void PrintResearchProgress_Test()
        {
            Debug.Log("현재 Research : "+selectedResearchId);
            var str = researchProgresses.Keys
                .Aggregate("", (current, key) => current + (key + " : " + researchProgresses[key] + "%   "));
            Debug.Log(str);
        }
        
        [ContextMenu("Test: 연구 선택 30001")]
        public void Test_SelectResearch0001()
        {
            SelectResearch(30001);
        }      
        
        [ContextMenu("Test: 연구 선택 30002")]
        public void Test_SelectResearch0002()
        {
            SelectResearch(30002);
        }
        #endregion
    }
}
