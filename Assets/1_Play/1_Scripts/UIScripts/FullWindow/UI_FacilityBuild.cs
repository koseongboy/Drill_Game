using System;
using System.Collections;
using System.Collections.Generic;
using DrillGame._1_Play._1_Scripts.Managers.Mono;
using DrillGame.UI;
using DrillGame.UI.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_FacilityBuild : MonoBehaviour, UI_IAddressable
    {
        #region Singleton & initialization
        public static UI_FacilityBuild Instance { get; private set; }
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
            
            InitCategoryObjectDict();
        }
        #endregion
        
        #region UI_Addressable
        [SerializeField]
        private string addressableName;
        public void LinkAddressable(string address)
        {
            addressableName = address;
        }
        #endregion
        
        #region Fields & Properties
        [SerializeField] private RectTransform contentRectTransform;
        [SerializeField] private GameObject ui_buildPiece;
        
        [SerializeField]
        private List<GameObject> categoryObjectList = new List<GameObject>();
        private Dictionary<Facility_Data_.FacilityType, GameObject> categoryObjectDict;
        private Dictionary<Facility_Data_.FacilityType, List<int>> facilityItemIds;
        #endregion

        #region getters & setters

        #endregion

        #region public methods
        public virtual void CloseUI()
        {
            CloseAction();
        }
        #endregion

        #region private methods

        private void InitCategoryObjectDict()
        {
            categoryObjectDict = new Dictionary<Facility_Data_.FacilityType, GameObject>();

            for (Facility_Data_.FacilityType i = 0; i < (Facility_Data_.FacilityType)5; i++)
            {
                categoryObjectDict[i] = categoryObjectList[(int)i];
            }
        }
        private void SetBuildPieces()
        {
            // 1. 카테고리별로, 개수에 맞게 프리팹을 생성 (일단 객체화. 풀 쓰지 말고)
            foreach (var kvp_Type in facilityItemIds)
            {
                var itemIdList = kvp_Type.Value;
                var count = itemIdList.Count;
                for (int i = 0; i < count; i++)
                {
                    var buildPiece = Instantiate(ui_buildPiece, categoryObjectDict[kvp_Type.Key].transform);
                    buildPiece.GetComponent<UI_BuildPiece>().SetData( itemIdList[i] );
                }
            }
            
            // 1-1. 풀링 구현
            
            // UI 레이아웃 크기 재조정
            StartCoroutine(UpdateLayout());
        }

        private IEnumerator UpdateLayout()
        {
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
        }
        
        private void OpenAction()
        {
            
        }
        private void CloseAction()
        {
            UILoader.Instance.HideUI(addressableName);
        }
        #endregion

        #region Unity event methods
        private void OnEnable()
        {
            facilityItemIds = FacilityBuildManager.Instance.GetUnlockedFacilityItemIds();
            SetBuildPieces();
            OpenAction();
        }
        #endregion
    }
}
