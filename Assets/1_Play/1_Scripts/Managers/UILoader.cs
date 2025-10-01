using System;
using System.Collections.Generic;
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
                    Debug.Log($"UI Ȱ��ȭ: {uiName}");
                }
                else
                {
                    Debug.LogWarning($"UI {uiName} �ν��Ͻ��� null �Դϴ�. �ٽ� �ε� �õ��մϴ�.");
                    loadedUIs.Remove(uiName);
                    LoadUI(uiName);
                }
            }
            else
            {
                Debug.LogWarning($"UI {uiName} �� �ε�� ���°� �ƴմϴ�. �ε� �õ��մϴ�.");
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
                    Debug.Log($"UI ��Ȱ��ȭ: {uiName}");
                }
                else
                {
                    Debug.LogWarning($"UI {uiName} �ν��Ͻ��� null �Դϴ�. ��ε� �õ��մϴ�. ��Ȳ Ȯ���� �ʿ��մϴ�.");
                    loadedUIs.Remove(uiName);
                    UnloadUI(uiName);
                }
            }
            else
            {
                Debug.LogWarning($"UI {uiName} �� �ε�� ���°� �ƴմϴ�. ��ε� �õ��մϴ�.");
            }
        }

        public void LoadUI(string uiName)
        {
            // Implement UI loading logic here
            Debug.Log($"UI �ε� �õ�: {uiName}");

            if(loadUIHandles.ContainsKey(uiName))
            {
                Debug.LogWarning($"UI {uiName} �� �̹� �ε�� �����Դϴ�");

                // ��ó���� �ʿ��
                return;
            }

            AsyncOperationHandle <GameObject> loadHandle = Addressables.LoadAssetAsync<GameObject>(uiName);
            loadUIHandles.Add(uiName, loadHandle);

            loadHandle.Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log($"UI �ε� ����: {uiName}");
                    GameObject uiPrefab = handle.Result;
                    GameObject uiInstance = Instantiate(uiPrefab, uiParentTransform);

                    UI_IAddressable addressableComponent = uiInstance.GetComponent<UI_IAddressable>();
                    addressableComponent?.LinkAddressable(uiName);

                    loadedUIs.Add(uiName, uiInstance);
                }
                else
                {
                    Debug.LogError($"UI �ε� ����: {uiName}");
                    loadUIHandles.Remove(uiName);

                    Addressables.Release(handle);   //gemini ������ �зδ� �ε� �����ص� �޸� ���� �����ϰ� �Ϸ���
                }
            };
        }

        public void UnloadUI(string uiName)
        {
            // Implement UI unloading logic here
            Debug.Log($"UI ��ε� �õ�: {uiName}");
            if (!loadUIHandles.ContainsKey(uiName))
            {
                Debug.LogWarning($"UI {uiName} �� �ε�� ���°� �ƴմϴ�");
                // ��ó���� �ʿ��
                return;
            }

            if (loadedUIs.ContainsKey(uiName))
            {
                GameObject uiInstance = loadedUIs[uiName];

                if(uiInstance != null)
                    Destroy(uiInstance);
                else
                    Debug.LogWarning($"UI {uiName} �ν��Ͻ��� �̹� �ı��� �����Դϴ� �ǵ��� �����ΰ���");

                loadedUIs.Remove(uiName);
            }
            AsyncOperationHandle<GameObject> loadHandle = loadUIHandles[uiName];
            Addressables.Release(loadHandle);
            loadUIHandles.Remove(uiName);
            Debug.Log($"UI ��ε� �Ϸ�: {uiName}");
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
