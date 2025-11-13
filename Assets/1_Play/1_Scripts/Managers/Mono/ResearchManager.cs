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
        private string selectedResearchId = "";
        [SerializeField]
        private float progressValue = 1f;
        
        private Dictionary<string, float> researchProgresses;
        private const string RESEARCH_ID_KEY = "ResearchId";
        private const string RESEARCH_PROGRESS_KEY = "ResearchProgressData";
        
        private List<IResearchObserver> researchObservers = new List<IResearchObserver>();

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
        public void SelectResearch(string researchId)
        {
            selectedResearchId = researchId;
            SaveResearchId();
            AlertObservers();
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
            AlertObservers();
        }

        public float AddResearchObserver(IResearchObserver observer)
        {
            researchObservers.Add(observer);
            researchProgresses.TryGetValue(selectedResearchId, out float progress);
            return progress;
        }

        public void AlertObservers()
        {
            researchProgresses.TryGetValue(selectedResearchId, out float progress);
            
            foreach (var observer in researchObservers)
            {
                observer.OnResearchProgressChanged(progress);
            }
        }


        
        #region private methods

        private void SaveResearchId()
        {
            PlayerPrefs.SetString(RESEARCH_ID_KEY, selectedResearchId);
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
            Debug.Log("InitializeProgressDict : "+RESEARCH_PROGRESS_KEY);
            
            // Test
            researchProgresses = new Dictionary<string, float> { { "0001", 0f }, { "0002", 0f }, { "0003", 0f } };
            
            // TODO : CSV 데이터 다 읽어와서, Key : ID | Value : 0f인 딕셔너리를 만들어줘야.
            SaveResearchId();
            SaveResearchProgressData();
        }
        
        private void LoadResearchKey()
        {
            selectedResearchId = PlayerPrefs.GetString(RESEARCH_ID_KEY, "");
        }
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
                researchProgresses = new Dictionary<string, float>();
                foreach (var kvPair in rawDict)
                {
                    // 2. 값 타입을 float으로 강제 변환
                    researchProgresses.Add(kvPair.Key, Convert.ToSingle(kvPair.Value));
                }
            }
            else { Debug.LogError("로드된 데이터가 Dictionary 형태가 아님. Json 저장 형식에 문제가 있나봅니다."); }
        }
        
        #endregion
        
        #region Unity event methods
        private void Start()
        {
            LoadResearchKey();
            LoadProgressDict();
            AlertObservers();
        }
        
        private void OnApplicationQuit()
        {
            SaveResearchProgressData();
        }
        #endregion


        #region DEV
        /// <summary>
        /// 연구 진척도를 모두 0으로 바꾸는 함수!!! DEV용
        /// </summary>
        public void ResetResearchProgress_Test()
        {
            List<string> keys = new List<string>(researchProgresses.Keys);

            foreach (string key in keys)
            {
                researchProgresses[key] = 0.0f;
            }
            Debug.Log("모든 연구 진척도가 0%로 초기화되었습니다.");
            AlertObservers();
            SaveResearchProgressData();
        }

        /// <summary>
        /// 연구 진척도 Dictionary 출력 함수. DEV용
        /// </summary>
        public void PrintResearchProgress_Test()
        {
            Debug.Log("현재 Research : "+selectedResearchId);
            var str = researchProgresses.Keys
                .Aggregate("", (current, key) => current + (key + " : " + researchProgresses[key] + "%   "));
            Debug.Log(str);
        }
        
        [ContextMenu("Test: 연구 선택 0001")]
        public void Test_SelectResearch0001()
        {
            SelectResearch("0001");
        }      
        
        [ContextMenu("Test: 연구 선택 0002")]
        public void Test_SelectResearch0002()
        {
            SelectResearch("0002");
        }
        #endregion
        
        #endregion
    }

    public interface IResearchObserver
    {
        public void OnResearchProgressChanged(float progress);
    }
}
