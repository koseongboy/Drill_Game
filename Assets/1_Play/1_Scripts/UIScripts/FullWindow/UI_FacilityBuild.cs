using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DrillGame._1_Play._1_Scripts.Managers.Mono;
using DrillGame.Core.Managers;
using DrillGame.Managers;
using DrillGame.UI;
using DrillGame.UI.Interface;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
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
        private List<GameObject> buildPieces = new List<GameObject>();

        #region DetailWindow

        private int facilityItemId;
        private int buildResourceItemId;
        private int buildResourceItemCount;
        
        [SerializeField] private GameObject detailWindow;
        
        [SerializeField] private TextMeshProUGUI ui_titleTxt;
        [SerializeField] private TextMeshProUGUI ui_descTxt;
        [SerializeField] private TextMeshProUGUI ui_inputItemTxt;
        [SerializeField] private TextMeshProUGUI ui_selectButtonTxt;

        private Action<int, int, int> OnSelectButtonPressed;
        
        #endregion
        #endregion

        #region getters & setters

        #endregion

        #region public methods
        public virtual void CloseUI()
        {
            ClearAllPieces();
            CloseAction();
        }
        
        public void OpenDetailWindow( int facilityItemId )
        {
            UpdateDetailWindow( facilityItemId );
            DetailWindowOpenAnimation();
        }

        public void CloseDetailWindow()
        {
            DetailWindowInit();
            DetailWindowCloseAnimation();
        }

        public void SelectButtonPressed()
        {
            OnSelectButtonPressed?.Invoke( facilityItemId, buildResourceItemId, buildResourceItemCount );
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
                    buildPieces.Add(buildPiece);
                }
            }
            
            // 1-1. 풀링 구현
            
            // UI 레이아웃 크기 재조정
            StartCoroutine(UpdateLayout());
        }

        private void ClearAllPieces()
        {
            foreach (var buildPiece in buildPieces)
            {
                Destroy(buildPiece);
            }
        }

        private IEnumerator UpdateLayout()
        {
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
        }
        
        private void OpenAction()
        {
            DetailWindowInit();
            detailWindow.SetActive(false);
        }
        private void CloseAction()
        {
            UILoader.Instance.HideUI(addressableName);
        }
        
        private void DetailWindowInit()
        {
            facilityItemId = 0;
            ui_titleTxt.text = "";
            ui_descTxt.text = "";
            ui_inputItemTxt.text = "";
            ui_selectButtonTxt.text = "설치";
            OnSelectButtonPressed = null;
        }
        
        private void DetailWindowOpenAnimation()
        {
            detailWindow.SetActive(true);
            RectTransform rt = detailWindow.GetComponent<RectTransform>();
            
            Vector2 startPos = new Vector2(0, -500f);
            Vector2 targetPos = new Vector2(startPos.x, startPos.y + 100f);
            rt.anchoredPosition = startPos;
            rt.DOAnchorPos(targetPos, 0.1f)
                .SetEase(Ease.OutBack);
            
            rt.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(Vector2.one, 0.1f)
                .SetEase(Ease.OutBack);
        }
        
        private void DetailWindowCloseAnimation()
        {
            RectTransform rt = detailWindow.GetComponent<RectTransform>();
            
            Vector2 startPos = rt.anchoredPosition;
            Vector2 targetPos = new Vector2(startPos.x, startPos.y - 100f);
            rt.anchoredPosition = startPos;
            rt.DOAnchorPos(targetPos, 0.1f)
                .SetEase(Ease.Linear);
            
            Vector3 targetScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(targetScale, 0.1f)
                .SetEase(Ease.Linear)
                .OnComplete(() => {
                    detailWindow.SetActive(false);
                });
        }

        private void UpdateDetailWindow( int itemId )
        {
            var facilityItemData = ScriptableObjectManager.Instance.GetData<Item_Data_>( itemId );
            var facilityData = ScriptableObjectManager.Instance.GetData<Facility_Data_>( facilityItemData.EntityId );
            
            ui_titleTxt.text = facilityData.DisplayName;
            ui_descTxt.text = facilityData.GetFacilityDesc();

            var inputItemData = ScriptableObjectManager.Instance.GetData<Item_Data_>( facilityData.BuildResourceId );
            ui_inputItemTxt.text = $"필요 자원 : {inputItemData.DisplayName} {facilityData.BuildResourceId}개";

            // DEV
            ui_selectButtonTxt.text = "설치";
            OnSelectButtonPressed = BuildFacility;
            facilityItemId = itemId;
            buildResourceItemId = facilityData.BuildResourceId;
            buildResourceItemCount = facilityData.BuildResourceCount;
            
            // ############ 보유 자원 체크 ############
            // if (InventoryManager.Instance.IsContainsItem(facilityData.BuildResourceId, inputItemCount))
            // {
            //     ui_selectButtonTxt.text = "설치";
            //     OnSelectButtonPressed = BuildFacility;
            // }
            // else
            // {
            //     ui_selectButtonTxt.text = "자원 부족";
            //     OnSelectButtonPressed = CantBuildFacility;
            // }
        } 

        private void BuildFacility( int facilityItemId , int resourceId, int resourceCount )
        {
            // 1. 자원을 소모해주고...
            // if (!InventoryManager.Instance.TryRemoveItem(resourceCount, resourceCount))
            // {
            //     Debug.LogWarning("아이템이 부족합니다. 엥? 어떻게 정상적으로 여기까지 진입했어요?");
            //     return;
            // }
            
            // 2. 배치모드 진입해서...
            GameManager.Instance.BatchEntity( facilityItemId );
            
            // 3. 이 UI를 꺼주고...
            CloseUI();
        }

        private void CantBuildFacility( int facilityItemId )
        {
            // TODO : 설치할 수 없다는 Alert창 띄우기
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
