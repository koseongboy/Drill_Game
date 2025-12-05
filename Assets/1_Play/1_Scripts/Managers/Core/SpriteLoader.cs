using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DrillGame.Managers
{
    public class SpriteLoader
    {
        #region Fields & Properties
        private Dictionary<string, Sprite> _spriteCache;
        private Dictionary<string, Task<Sprite>> _loadingTasks;
        #endregion

        #region Singleton & initialization
        private static SpriteLoader instance;
        public static SpriteLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpriteLoader();
                }
                return instance;
            }
        }

        private SpriteLoader()
        {
            _spriteCache = new Dictionary<string, Sprite>();
            _loadingTasks = new Dictionary<string, Task<Sprite>>();
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods

        public async Task<Sprite> LoadSprite(string addressableName)
        {
            if (_spriteCache.TryGetValue(addressableName, out Sprite cachedSprite))
            {
                return cachedSprite;
            }
            if (_loadingTasks.TryGetValue(addressableName, out Task<Sprite> ongoingTask))
            {
                return await ongoingTask;
            }
            
            Task<Sprite> loadTask = InternalLoadSpriteAsync(addressableName);
            _loadingTasks.Add(addressableName, loadTask);
            
            try
            {
                return await loadTask;
            }
            finally
            {
                _loadingTasks.Remove(addressableName);
            }
        }
        #endregion

        #region private methods
        private async Task<Sprite> InternalLoadSpriteAsync(string addressableName)
        {
            var handle = Addressables.LoadAssetAsync<Sprite>(addressableName);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite icon = handle.Result;
                _spriteCache.Add(addressableName, icon); 
                return icon;
            }
            else
            {
                Debug.LogError($"FAILURE: Addressables 로드 실패. 주소: '{addressableName}'");
                return null;
            }
        }
        #endregion

        #region Unity event methods
        #endregion

    }
}
