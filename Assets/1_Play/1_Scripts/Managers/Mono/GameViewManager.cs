using UnityEngine;

namespace DrillGame.Managers
{
    public class GameViewManager : MonoBehaviour
    {
        public enum ViewState
        {
            All,
            EngineOnly,
            FacilityOnly
        }

        #region Fields & Properties
        public Transform engineViewParent;
        public Transform facilityViewParent;

        #endregion

        #region Singleton & initialization

        public static GameViewManager Instance { get; private set; }
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

        #region getters & setters
        #endregion

        #region public methods
        public void SetViewState(ViewState state)
        {
            // Implement view state change logic here
            Debug.Log($"View state changed to: {state}");

            switch (state)
            {
                case ViewState.All:
                    engineViewParent.gameObject.SetActive(true);
                    facilityViewParent.gameObject.SetActive(true);
                    break;
                case ViewState.EngineOnly:
                    engineViewParent.gameObject.SetActive(true);
                    facilityViewParent.gameObject.SetActive(false);
                    break;
                case ViewState.FacilityOnly:
                    engineViewParent.gameObject.SetActive(false);
                    facilityViewParent.gameObject.SetActive(true);
                    break;
            }
        }

        // int 0 -> All, 1 -> EngineOnly, 2 -> FacilityOnly
        public void SetViewState(int index)
        {
             SetViewState((ViewState)index);
        }

        public void Test()
        {
            Debug.Log("GameViewManager Test method called.");
        }

        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}
