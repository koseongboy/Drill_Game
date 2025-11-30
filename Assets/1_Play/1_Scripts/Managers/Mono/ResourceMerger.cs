using DrillGame.Core.Managers;
using UnityEngine;

namespace DrillGame._1_Play._1_Scripts.Managers.Mono
{
    public class ResourceMerger_SaveData
    {
        public int inputItemId;
        public int outputItemId;

        public ResourceMerger_SaveData( int inputItemId, int outputItemId )
        {
            this.inputItemId = inputItemId;
            this.outputItemId = outputItemId;
        }
    }

    public class ResourceMerger : MonoBehaviour
    {
        #region Singleton & initialization
        public static ResourceMerger Instance { get; private set; }
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

            LoadResourceMergerDataFromES3();
        }
        #endregion

        #region Fields & Properties

        [SerializeField] private int inputItemId = 0;
        [SerializeField] private int outputItemId = 0;

        [SerializeField] private int inputCount = 1; // TODO : CSV의 FacilityLevel에 따라 다른 값 사용하기.
        [SerializeField] private int outputCount = 1; // TODO

        private const string RESOURCE_MERGER_KEY = "ResourceMergerData";
        #endregion

        #region getters & setters
        #endregion

        #region public methods

        // Facility에서 호출될 함수
        // Facility에서 호출될 함수
        // Facility에서 호출될 함수
        public void RunProcess()
        {
            if (!InventoryManager.Instance.TryRemoveItem(inputItemId, inputCount))
            {
                // TODO : 지금 자원이 부족해서 놀고있다는 걸 알려줘야겠지?
                return;
            }
            
            InventoryManager.Instance.AddItem(outputItemId, outputCount);
        }
        #endregion

        #region private methods
        private void LoadResourceMergerDataFromES3()
        {
            var data = ES3.Load(RESOURCE_MERGER_KEY, new ResourceMerger_SaveData(0, 0));
            inputItemId = data.inputItemId;
            outputItemId = data.outputItemId;
        }
        
        private void SaveResourceMergerData()
        {
            var data = new ResourceMerger_SaveData( inputItemId, outputItemId );
            ES3.Save(RESOURCE_MERGER_KEY, data);
        }
        #endregion

        #region Unity event methods
        private void OnApplicationQuit()
        {
            SaveResourceMergerData();
        }
        #endregion

        #region DEV
        #endregion
    }
}