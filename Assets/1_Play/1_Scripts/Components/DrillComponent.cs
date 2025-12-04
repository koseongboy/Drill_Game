using System.Collections;
using System.Collections.Generic;
using DrillGame.View.Ground;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DrillGame.View.Drill
{
    public class DrillComponent : MonoBehaviour
    {
        #region Fields & Properties
        SpriteRenderer spriteRenderer;
        Drill_Data_ Currentdata;
        Sprite CurrentSprite;

        private const string ES3FILENAME = "DrillUserData.es3";
        private const string DRILL_LEVEL = "DrillLevel";

        private int drillLevel;

        #endregion

        #region Singleton & initialization
        private static DrillComponent instance;
        public static DrillComponent Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<DrillComponent>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            
            spriteRenderer = GetComponent<SpriteRenderer>();
            ES3File es3File = new ES3File(ES3FILENAME);
            drillLevel = es3File.Load(DRILL_LEVEL, 1);
            Currentdata = ScriptableObjectManager.Instance.GetData<Drill_Data_>(drillLevel + 3000);
            Debug.Log("드릴 컴포넌트 초기화. 현재 레벨: " + drillLevel);
            if(Currentdata != null)
            Debug.Log("드릴 데이터 로드 완료. 현재 드릴 ID: " + Currentdata.Id);
            else
            Debug.LogError("드릴 데이터 로드 실패!");
            //드릴 스프라이트 초기화
            LoadSprite();
        }
        #endregion

        #region getters & setters
        public int GetDrillLevel()
        {
            return drillLevel;
        }
        public int GetDrillDamage()
        {
            return Currentdata.Damage;
        }
        
        #endregion

        #region public methods
        public async void LoadSprite()
        {

            Debug.Log("드릴 스프라이트 로드 시작. 현재 드릴 어드레서블 주소: " + Currentdata.DrillSprite);
            var currentHandle = Addressables.LoadAssetAsync<Sprite>(Currentdata.DrillSprite);
            CurrentSprite = await currentHandle.Task;
            spriteRenderer.sprite = CurrentSprite;
        }

        public void levelUp(int toWhat)
        {
            try
            {
                Drill_Data_ temp = ScriptableObjectManager.Instance.GetData<Drill_Data_>(3000 + toWhat);
                Currentdata = temp;
            } catch
            {
                Debug.Log("그런 레벨은 존재하지 않습니다. 아무 행동도 취하지 않습니다.");
                return;
            }
            drillLevel = toWhat;
            LoadSprite();
            SaveDrillData(drillLevel);
            Debug.Log("드릴 레벨 업! 현재 레벨: " + drillLevel);
        }

        public void oneLevelUp()
        {
            levelUp(drillLevel + 1);
        }
        

        public void resetData()
        {
            if (ES3.FileExists(ES3FILENAME))
            {
                // 2. 파일이 존재하면 ES3.DeleteFile()로 삭제합니다.
                ES3.DeleteFile(ES3FILENAME);

                Debug.Log($"[ES3 Reset] 세이브 파일 '{ES3FILENAME}'이 성공적으로 삭제되었습니다.");
                
                // 3. (선택 사항) 삭제 후 초기 상태로 게임을 재시작하거나 로드할 수 있습니다.
                // SceneManager.LoadScene(0);
            }
            else
            {
                Debug.LogWarning($"[ES3 Reset] 삭제할 세이브 파일 '{ES3FILENAME}'이 존재하지 않습니다. 이미 초기화된 상태일 수 있습니다.");
            }
        }
        #endregion

        #region private methods
        private void SaveDrillData(int level)
        {
            ES3File es3File = new ES3File(ES3FILENAME);
            es3File.Save(DRILL_LEVEL, level);
            es3File.Sync();
        }
        #endregion

        #region Unity event methods

        private void Start()
        {
        
        }

        private void Update()
        {
        
        }
        #endregion
    }
}