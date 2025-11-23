using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MiniJSON;
using UnityEngine.InputSystem;

namespace DrillGame
{
    public class ResearchManager : MonoBehaviour
    {
        [SerializeField]
        private int selectedResearchId = 0;
        [SerializeField]
        private float progressValue = 1f;
        
        private Dictionary<int, float> researchProgresses;
        private const string RESEARCH_SELECTED_ID_KEY = "ResearchId";
        private const string RESEARCH_PROGRESS_KEY = "ResearchProgressData";
        public event Action<int, float> OnResearchProgressChanged;

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
        
        #region public methods
        public void SelectResearch(int researchId)
        {
            selectedResearchId = researchId;
            SaveResearchId();
            OnResearchProgressChanged?.Invoke( selectedResearchId, researchProgresses[selectedResearchId] );
            // AlertObservers();
        }
        
        public void AddResearchProgress()
        {
            if (!researchProgresses.ContainsKey(selectedResearchId))
            {
                Debug.Log("올바르지 않은 ResearchKey입니다. : "+ selectedResearchId);
                return;
            }
            researchProgresses[selectedResearchId] += progressValue;
            if (researchProgresses.Count > 100)
            {
                researchProgresses[selectedResearchId] = 100f;
            }
            OnResearchProgressChanged?.Invoke( selectedResearchId, researchProgresses[selectedResearchId] );
        }
        
        
        #region private methods

        private void SaveResearchId()
        {
            PlayerPrefs.SetInt(RESEARCH_SELECTED_ID_KEY, selectedResearchId);
            PlayerPrefs.Save();
        }
        
        private void SaveResearchProgressData()
        {
            // TODO : 주기적으로 (10 코어틱?) 이거 호출해서 저장해줘야하지 않을까?
            string jsonString = Json.Serialize(researchProgresses);
            
            PlayerPrefs.SetString(RESEARCH_PROGRESS_KEY, jsonString);
            PlayerPrefs.Save(); 
            
            // Debug.Log("연구 진척도를 저장했습니다.");
        }

        private void InitializeProgressDict()
        {
            var researchDatas = ScriptableObjectManager.Instance.GetAllData<Research_Data_>();
            researchProgresses = new Dictionary<int, float>();
            foreach (var researchData in researchDatas)
            {
                researchProgresses.Add(researchData.Key, 0f);
            }
            
            SaveResearchId();
            SaveResearchProgressData();
        }
        
        private void LoadResearchKey()
        {
            selectedResearchId = PlayerPrefs.GetInt(RESEARCH_SELECTED_ID_KEY, 1);
        }
        
        [ContextMenu("Load Progress Dict - ScriptableData 추가되면 실행")]
        private void LoadProgressDict()
        {
            string jsonString = PlayerPrefs.GetString(RESEARCH_PROGRESS_KEY, null);
            
            if (jsonString is null or "null")
            {
                Debug.Log("Json이 Null임. Dictionary를 새로 생성합니다.");
                InitializeProgressDict(); // 저장된 데이터가 없으니, Init
                return;
            }
            
            var data = Json.Deserialize(jsonString);
            if (data is Dictionary<string, object> rawDict) {
                researchProgresses = new Dictionary<int, float>();
                foreach (var kvPair in rawDict)
                {
                    // 2. 값 타입을 float으로 강제 변환
                    researchProgresses.Add(int.Parse(kvPair.Key), Convert.ToSingle(kvPair.Value));
                }
            }
            else { Debug.LogError("Json 저장 형식에 문제가 있나봅니다. 로드 중 오류가 발생했습니다"); }
        }
        
        #endregion
        
        #region Unity event methods
        private void Start()
        {
            LoadResearchKey();
            LoadProgressDict();
            OnResearchProgressChanged?.Invoke( selectedResearchId, researchProgresses[selectedResearchId] );
            // AlertObservers();
        }
        
        private void OnApplicationQuit()
        {
            SaveResearchProgressData();
        }
        #endregion


        #region DEV

        [ContextMenu("PlayerPref 키 삭제")]
        public void DeleteResearchDataInPlayerPref()
        {
            PlayerPrefs.DeleteKey(RESEARCH_PROGRESS_KEY);
            PlayerPrefs.Save();
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
            OnResearchProgressChanged?.Invoke( selectedResearchId, researchProgresses[selectedResearchId] );
            SaveResearchProgressData();
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
        
        #endregion
    }
}
