using DrillGame.Core.Managers;
using DrillGame.UI;
using UnityEngine;

namespace DrillGame
{
    public class ResourceConverterManager : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField] private int targetItemId;
        [SerializeField] private int inputItemId;
        [SerializeField] private int exchangeAmount = 1;

        #endregion

        #region Singleton & initialization
        public static ResourceConverterManager Instance { get; private set; }
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

        private void Init()
        {
            targetItemId = 0;
            inputItemId = 0;
        }
        #endregion

        #region getters & setters

        public void SetTargetItemId( int id )
        {
            targetItemId = id;
            inputItemId = id - 2;
        }
        #endregion

        #region public methods

        public void RunResourceMergeProcess()
        {
            if (targetItemId == 0)
            {
                return;
            }

            if (!InventoryManager.Instance.TryRemoveItem(inputItemId, exchangeAmount))
            {
                UILoader.Instance.ShowAlert("자원 변환기가 멈췄습니다. 재료 자원이 부족합니다!");
                // TODO : 이슈 UI에 추가하기
                Init();
                return;
            }
            InventoryManager.Instance.AddItem(targetItemId, exchangeAmount);
        }
        #endregion

        #region private methods
            
        #endregion

        #region Unity event methods

        #endregion

        #region DEV

        #endregion
    }
}
