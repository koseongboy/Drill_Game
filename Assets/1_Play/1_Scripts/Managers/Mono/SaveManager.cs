using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.Managers
{
    public class SaveManager : MonoBehaviour
    {
        #region Fields & Properties
        public static SaveManager Instance { get; private set; }

        private const string IS_FIRST_LAUNCH_KEY = "IsFirstLaunch";

        public Dictionary<string, string> SaveKeys = new Dictionary<string, string>()
        {
            { "RESEARCH_SELECTED_ID_KEY", "ResearchId" },
            { "RESEARCH_PROGRESS_KEY", "ResearchProgressData" },
            { "ENGINE_MERGER_KEY", "EngineMergerData" },

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


            Debug.Log("첫 실행 데이터가 설정되었습니다.");
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
        #endregion

        #region Unity event methods
        #endregion
    }
}
