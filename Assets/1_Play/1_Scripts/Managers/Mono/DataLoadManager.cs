using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using DrillGame.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DrillGame.Managers
{
    public class DataLoadManager : MonoBehaviour
    {
        #region Fields & Properties
        public Engine_Data EngineData { get; set; }
        public Facility_Data FacilityData { get; set; }
        public Ground_Data GroundData { get; set; }
        public Sprite CurrentGroundSprite { get; set; }
        public Sprite NextGroundSprite { get; set; }
        public List<int> DepthRanges { get; set; }
        public Dictionary<string, List<string>> UserData { get; set; } = new Dictionary<string, List<string>>()
            {
                { "Engine", new List<string> { "normal-2", "special-1" } },
                { "Facility", new List<string> { "iron-1", "gold-1" } },
                { "Ground", new List<string> { "3", "5"} } //depth, hp
            };//임시!!!'

        private AsyncOperationHandle<Sprite> currentGroundHandle;
        private AsyncOperationHandle<Sprite> nextGroundHandle;

        #endregion

        #region Singleton & initialization
        public static DataLoadManager Instance;
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        //다음 땅 스프라이트만 로드
        public async void LoadGroundSpriteAsync(string next)
        {
            Addressables.Release(currentGroundHandle);
            currentGroundHandle = nextGroundHandle;
            CurrentGroundSprite = NextGroundSprite;
            nextGroundHandle = Addressables.LoadAssetAsync<Sprite>(next);
            await Task.WhenAll(nextGroundHandle.Task);
            NextGroundSprite = nextGroundHandle.Result;
            Debug.Log("다음 땅 엑셀 정보: " + next);
            Debug.Log("다음 땅 스프라이트: " + NextGroundSprite.name);
        }
        //초기 땅 스프라이트 로드
        public async Task<bool> LoadGroundSpriteAsync(string current, string next)
        {
            currentGroundHandle = Addressables.LoadAssetAsync<Sprite>(current);
            nextGroundHandle = Addressables.LoadAssetAsync<Sprite>(next);
            await Task.WhenAll(currentGroundHandle.Task, nextGroundHandle.Task);
            CurrentGroundSprite = currentGroundHandle.Result;
            NextGroundSprite = nextGroundHandle.Result;
  

            return CurrentGroundSprite != null && NextGroundSprite != null;
        }

        //depth가 현재 속한 땅 구간의 start_depth, 다음 구간의 start_depth 반환
        public List<int> GetRangeBounds(int depth)
        {
            foreach (int start_depth in DepthRanges.AsEnumerable().Reverse()) //역순으로 순회
            {
                if (depth < start_depth)
                {
                    continue;
                }
                return new List<int> { start_depth, int.Parse(GroundData.Table[start_depth]["End Depth"]) + 1 };
            }
            Debug.LogError("GetRangeInfo error: depth range not found for depth " + depth + " in " + DepthRanges.Count());
            return null;
        }
        #endregion

        #region private methods

        private async Task<bool> LoadAllDataAsync()
        {
            var engine_Data = Engine_Data.CreateAsync();
            var facility_Data = Facility_Data.CreateAsync();
            var ground_Data = Ground_Data.CreateAsync();
            await Task.WhenAll(engine_Data, facility_Data, ground_Data);
            EngineData = engine_Data.Result;
            FacilityData = facility_Data.Result;
            GroundData = ground_Data.Result;
            DepthRanges = ground_Data.Result.DepthRanges;
            List<int> range = GetRangeBounds(int.Parse(UserData["Ground"][0])); //임시 유저 데이터 불러옴
            var result = await LoadGroundSpriteAsync(GroundData.Table[range[0]]["Sprite Addressable"], GroundData.Table[range[1]]["Sprite Addressable"]);
            return EngineData != null && FacilityData != null && GroundData != null && result;
        }
        
        #endregion

        #region Unity event methods
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                transform.parent = null;
                DontDestroyOnLoad(this.gameObject);
            }
            // Debug.Log("DataLoadManager Awake completed.");
        }
        async void Start()
        {
            if (await LoadAllDataAsync())
            {
                Debug.Log("[DataLoadManager] All CSV_Data has been loaded successfully.");
            }
            else
            {
                Debug.LogError("[DataLoadManager] Failed to load Data.");
            }
        }

        private void OnDestroy()
        {
            Addressables.Release(currentGroundHandle);
            Addressables.Release(nextGroundHandle);
            Debug.Log("스프라이트 어드레서블 해제됨");
        }
        #endregion
  }
}
