using System;
using System.Collections.Generic;
using DrillGame.Core.Engine;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using DrillGame.UI .Interface;



namespace DrillGame.UI
{
    public class UILoader : MonoBehaviour
    {
        #region Fields & Properties
        [SerializeField]
        private Transform uiParentTransform;    
        
        [SerializeField]
        private Transform floatingBarUIParentTransform;

        [SerializeField] 
        private GameObject devGrid;

        [SerializeField]
        private List<string> awakeUIList = new List<string>();
        
        private Dictionary<string, AsyncOperationHandle<GameObject>> loadUIHandles = new();
        private Dictionary<string, GameObject> loadedUIs = new();

        #endregion

        #region Singleton & initialization
        public static UILoader Instance { get; private set; }
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

        public void ShowUI(string uiName)
        {
            if(loadedUIs.ContainsKey(uiName))
            {
                GameObject uiInstance = loadedUIs[uiName];
                if(uiInstance != null)
                {
                    uiInstance.SetActive(true);
                    Debug.Log($"UI 활성화: {uiName}");
                }
                else
                {
                    Debug.LogWarning($"UI {uiName} 인스턴스가 null 입니다. 다시 로드 시도합니다.");
                    loadedUIs.Remove(uiName);
                    LoadUI(uiName);
                }
            }
            else
            {
                // Debug.LogWarning($"UI {uiName} 는 로드된 상태가 아닙니다. 로드 시도합니다.");
                LoadUI(uiName);
            }
        }

        public void HideUI(string uiName)
        {
            if (loadedUIs.ContainsKey(uiName))
            {
                GameObject uiInstance = loadedUIs[uiName];
                if (uiInstance != null)
                {
                    uiInstance.SetActive(false);
                    Debug.Log($"UI 비활성화: {uiName}");
                }
                else
                {
                    Debug.LogWarning($"UI {uiName} 인스턴스가 null 입니다. 언로드 시도합니다. 상황 확인이 필요합니다.");
                    loadedUIs.Remove(uiName);
                    UnloadUI(uiName);
                }
            }
            else
            {
                Debug.LogWarning($"UI {uiName} 는 로드된 상태가 아닙니다. 언로드 시도합니다.");
            }
        }

        public void LoadUI(string uiName)
        {
            // Implement UI loading logic here
            // Debug.Log($"UI 로드 시도: {uiName}");

            if(loadUIHandles.ContainsKey(uiName))
            {
                Debug.LogWarning($"UI {uiName} 는 이미 로드된 상태입니다");

                // 후처리가 필요시
                return;
            }

            AsyncOperationHandle <GameObject> loadHandle = Addressables.LoadAssetAsync<GameObject>(uiName);
            loadUIHandles.Add(uiName, loadHandle);

            loadHandle.Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    // Debug.Log($"UI 로드 성공: {uiName}");
                    GameObject uiPrefab = handle.Result;
                    if (uiName == "UI_FloatingBar") {
                        GameObject uiInstance = Instantiate(uiPrefab, floatingBarUIParentTransform);
                        
                        UI_IAddressable addressableComponent = uiInstance.GetComponent<UI_IAddressable>();
                        addressableComponent?.LinkAddressable(uiName);

                        loadedUIs.Add(uiName, uiInstance);
                    }
                    else { // 하씨 이게 맞나
                        GameObject uiInstance = Instantiate(uiPrefab, uiParentTransform);

                        UI_IAddressable addressableComponent = uiInstance.GetComponent<UI_IAddressable>();
                        addressableComponent?.LinkAddressable(uiName);

                        loadedUIs.Add(uiName, uiInstance);
                    }
                    

                }
                else
                {
                    Debug.LogError($"UI 로드 실패: {uiName}");
                    loadUIHandles.Remove(uiName);

                    Addressables.Release(handle);   //gemini 선생님 왈로는 로드 실패해도 메모리 해제 안전하게 하래요
                }
            };
        }

        public void UnloadUI(string uiName)
        {
            // Implement UI unloading logic here
            // Debug.Log($"UI 언로드 시도: {uiName}");
            if (!loadUIHandles.ContainsKey(uiName))
            {
                Debug.LogWarning($"UI {uiName} 는 로드된 상태가 아닙니다");
                // 후처리가 필요시
                return;
            }

            if (loadedUIs.ContainsKey(uiName))
            {
                GameObject uiInstance = loadedUIs[uiName];

                if(uiInstance != null)
                    Destroy(uiInstance);
                else
                    Debug.LogWarning($"UI {uiName} 인스턴스가 이미 파괴된 상태입니다 의도된 사항인가요");
                
                loadedUIs.Remove(uiName);
            }
            AsyncOperationHandle<GameObject> loadHandle = loadUIHandles[uiName];
            Addressables.Release(loadHandle);
            loadUIHandles.Remove(uiName);

            Debug.Log($"UI 언로드 완료: {uiName}");
        }

        public void ShowUI_FacilityDetail()
        {
            ShowUI("FacilityDetail");
        }
        
        public void ShowUI_EngineDetail(EngineEntity engine)
        {

        }

        public void ToggleDevGrid() { //TODO : F1 누르면 Dev Grid 활성화 토글
            devGrid.SetActive(!devGrid.activeSelf);
        }
        #endregion

        #region private methods

        /// <summary>
        /// 씬이 시작될 때 Show되어야할 UI들을 Show
        /// </summary>
        private void ShowUIOnSceneStart()
        {
            foreach (var uiName in awakeUIList)
            {
                ShowUI(uiName);
            }
        }
        
        #endregion

        #region Unity event methods

        private void Start()
        {
            ShowUIOnSceneStart();
        }

        #endregion


    }
}
