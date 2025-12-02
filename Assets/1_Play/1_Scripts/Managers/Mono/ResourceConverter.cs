using DrillGame.Core.Managers;
using DrillGame.UI;
using UnityEngine;

namespace DrillGame.Managers
{
    public class ResourceConverter_SaveData
    {
        public int inputItemId;
        public int outputItemId;

        public ResourceConverter_SaveData( int inputItemId, int outputItemId )
        {
            this.inputItemId = inputItemId;
            this.outputItemId = outputItemId;
        }
    }

    public class ResourceConverter : MonoBehaviour
    {
        #region Singleton & initialization
        public static ResourceConverter Instance { get; private set; }
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

            LoadResourceConverterDataFromES3();
        }

        private void Init()
        {
            outputItemId = 0;
            inputItemId = 0;
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

        public void SetOutputItemId(int outputItemId)
        {
            this.outputItemId = outputItemId;
            this.inputItemId = outputItemId - 2;
        }
        #endregion

        #region public methods

        // Facility에서 호출될 함수
        // Facility에서 호출될 함수
        // Facility에서 호출될 함수
        public void RunProcess()
        {
            if (outputItemId == 0)
            {
                return;
            }
            if (!InventoryManager.Instance.TryRemoveItem(inputItemId, inputCount))
            {
                UILoader.Instance.ShowAlert("자원 합성기가 멈췄습니다. 재료 자원이 부족합니다!");
                // TODO : 이슈 UI에 추가하기
                Init();
                return;
            }
            
            InventoryManager.Instance.AddItem(outputItemId, outputCount);
        }
        #endregion

        #region private methods
        private void LoadResourceConverterDataFromES3()
        {
            var data = ES3.Load(RESOURCE_MERGER_KEY, new ResourceConverter_SaveData(0, 0));
            inputItemId = data.inputItemId;
            outputItemId = data.outputItemId;
        }
        
        private void SaveResourceConverterData()
        {
            var data = new ResourceConverter_SaveData( inputItemId, outputItemId );
            ES3.Save(RESOURCE_MERGER_KEY, data);
        }
        #endregion

        #region Unity event methods
        private void OnApplicationQuit()
        {
            SaveResourceConverterData();
        }
        #endregion

        #region DEV
        #endregion
    }
}