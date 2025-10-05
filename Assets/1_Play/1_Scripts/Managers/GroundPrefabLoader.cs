using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DrillGame.Managers
{
    public class GroundPrefabLoader : MonoBehaviour
    {
        #region Fields & Properties

        private GameObject groundPrefab;
        #endregion

        #region Singleton & initialization
        public static GroundPrefabLoader Instance;
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        async public void LoadGroundPrefabAsync()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("Ground_Prefab");
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                groundPrefab = handle.Result;
                Debug.Log("Ground prefab loaded successfully.");
                DataLoadManager.Instance.SetGroundPrefab(handle.Result);
            }
            else
            {
                Debug.LogError("Failed to load ground prefab.");
            }
        }
        public void ReleaseGroundPrefab()
        {
            if (groundPrefab != null)
            {
                Addressables.Release(groundPrefab);
                groundPrefab = null;
                Debug.Log("Ground prefab released.");
            }
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                LoadGroundPrefabAsync();
                DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
        
        }

        private void Update()
        {
        
        }
        #endregion
    }
}