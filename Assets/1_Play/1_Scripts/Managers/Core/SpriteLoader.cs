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
            Sprite icon = null;
            
            var handle = Addressables.LoadAssetAsync<Sprite>(addressableName);
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                icon = handle.Result;
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

        #region private methods
        #endregion

        #region Unity event methods
        #endregion

    }
}
