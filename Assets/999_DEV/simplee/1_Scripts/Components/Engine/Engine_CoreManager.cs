using UnityEngine;


using DrillGame.Entity.Engine;
using System.Collections;

namespace DrillGame.Components.Engine
{
    public class Engine_CoreManager : MonoBehaviour
    {
        #region Fields & Properties
        private static int _instanceCount = 0;

        private const float TICK_INTERVAL = 1f; // Tick 간격 (초)

        private Engine_Core core;
        private Coroutine coreTickCoroutine;
        

        #endregion

        #region Singleton & initialization
        private void Initialize()
        {
            core = Engine_Core.Instance;
            coreTickCoroutine = null;
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void ActivateCoreTickCorutine()
        {
            Debug.Log("Engine_Core Tick Coroutine Activated.");
            // Coroutine logic here

            if(coreTickCoroutine != null)
            {
                Debug.LogWarning("코어 틱 코루틴이 이미 존재합니다. 원인을 찾아봐야합니다.");
            }
            coreTickCoroutine = StartCoroutine(CoreTickCoroutine());
        }
        #endregion

        #region private methods
        private IEnumerator CoreTickCoroutine()
        {
            while (true)
            {
                core.AddTickCount();        // Tick 카운트 증가
                core.ActivateCore();        // 코어(모든 엔진) 활성화

                yield return new WaitForSeconds(TICK_INTERVAL); // Example: wait for 1 second between ticks
            }
        }
        #endregion

        #region Unity event methods
        private void Awake()
        {
            _instanceCount++;

            if (_instanceCount > 1)
            {
                Debug.LogError($"씬에 {nameof(Engine_CoreManager)}가 1개 이상 존재합니다. 중복 객체({this.name})를 파괴합니다.");

                // 중복된 자신(this)을 파괴
                Destroy(this.gameObject);

                // 카운트를 다시 감소시켜 실제 존재하는 개수를 맞춤
                _instanceCount--;

                return;
            }

            Initialize();
        }
        #endregion
    }

}
