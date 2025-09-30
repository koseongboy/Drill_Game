using DG.Tweening;
using DrillGame.Entity.Engine;
using System.Collections;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

namespace DrillGame.Components.Engine
{
    public class Engine_CoreManager : MonoBehaviour
    {
        #region Fields & Properties
        private static int _instanceCount = 0;

        // test 후일 조정 필요
        [SerializeField]
        private float TICK_INTERVAL = 1f; // Tick 간격 (초)

        private Engine_Core core;
        private Coroutine coreTickCoroutine;

        [ReadOnly]
        [SerializeField]
        private long tickCount = 0;

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
                // test
                // warning: ulong to long 변환시 데이터 손실 가능성 있음
                tickCount = (long)core.totalTickCount; // 현재 Tick 카운트 가져오기 (디버그용)
                UpdateCoreObject();         // 코어 게임오브젝트 업데이트
                core.ActivateCore();        // 코어(모든 엔진) 활성화 -> pure c# logic

                yield return new WaitForSeconds(TICK_INTERVAL); // Example: wait for 1 second between ticks
            }
        }

        private void UpdateCoreObject()
        {
            Debug.Log($"Engine_CoreManager: 코어 오브젝트 업데이트. ");
            TempGraphicAction();
        }
        private void TempGraphicAction()
        {
            // Vector3.one * 0.15f는 모든 축으로 0.15만큼 펀치한다는 의미입니다.
            transform.DOPunchScale(
                punch: Vector3.one * 0.2f, // 현재 크기에서 20% 커짐
                duration: 0.15f,             // 0.15초 동안 커졌다 돌아옴
                vibrato: 1,                 // 한 번만 튀어나왔다 돌아오도록 설정
                elasticity: 1               // 탄성력 설정 (1이 일반적으로 부드러움)
            );
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

        

        private void Start()
        {
            ActivateCoreTickCorutine();
        }
        #endregion
    }

}
