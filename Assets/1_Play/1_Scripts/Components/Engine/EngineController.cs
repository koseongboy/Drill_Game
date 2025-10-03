using DG.Tweening;
using DrillGame.Entity;
using DrillGame.Entity.Engine;
using System;
using System.Collections;
using UnityEngine;

namespace DrillGame.Components.Engine
{
    public class EngineController : ComponentBaseController
    {
        #region Fields & Properties
        private Engine_Base engineEntity;

        [ReadOnly]
        [SerializeField]
        public float engineWaitDelay;

        #endregion

        #region Singleton & initialization
        public override void Initialize(Entity_Base entity)
        {
            base.Initialize(entity);

            

            engineEntity = entity as Engine_Base;
            engineEntity.OnEngineRequestActivated += HandleCoroutineRequest;
            engineEntity.OnActivated+= HandleEngineActivated;

            engineEntity.OnUpdated += HandleUpdated;

            HandleUpdated(); // 초기값 세팅
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        
        #endregion

        #region private methods
        private void HandleCoroutineRequest(IEnumerator enumerator, Action callback)
        {
            StartCoroutine(WaitAndActivate(enumerator, callback));
        }
        private void HandleEngineActivated()
        {
            Debug.Log($"mono - {entityName} 엔진이 active 되었습니다.");
            UpDateEngineObject();
        }
        private void UpDateEngineObject()
        {
             TempGraphicAction();
        }

        protected override void HandleUpdated()
        {
            base.HandleUpdated();
            this.engineWaitDelay = engineEntity.waitDelay;
        }

        private IEnumerator WaitAndActivate(IEnumerator routine, Action callback)
        {
            // 1. 순수 C#에서 전달받은 코루틴을 실행하고 끝날 때까지 대기 (대기 시간)
            yield return StartCoroutine(routine);

            // 2. 코루틴이 완료되면 콜백 Action 실행 (엔진 작동)
            callback?.Invoke();
        }


        private void TempGraphicAction()
        {
            Renderer targetRenderer = GetComponent<Renderer>();
            Color flashColor = Color.lightSkyBlue; // 원하는 색상으로 변경 가능
            float halfDuration = 0.1f; // 전체 지속 시간
            Color originalColor = targetRenderer.material.color;

            // 시퀀스 생성
            Sequence colorSequence = DOTween.Sequence();

            // 1단계: 새 색깔로 변경
            colorSequence.Append(
                targetRenderer.material.DOColor(flashColor, halfDuration)
            );

            // 2단계: 원래 색깔로 복귀
            colorSequence.Append(
                targetRenderer.material.DOColor(originalColor, halfDuration)
            );

            colorSequence.Play();

            // Vector3.one * 0.15f는 모든 축으로 0.15만큼 펀치한다는 의미입니다.
            //transform.DOPunchScale(
            //    punch: Vector3.one * 0.2f, // 현재 크기에서 20% 커짐
            //    duration: 0.15f,             // 0.15초 동안 커졌다 돌아옴
            //    vibrato: 1,                 // 한 번만 튀어나왔다 돌아오도록 설정
            //    elasticity: 1               // 탄성력 설정 (1이 일반적으로 부드러움)
            //);
        }
        #endregion

        #region Unity event methods

        private void Awake()
        {
            if (entityName == null)
            {
                Debug.LogWarning("EngineController에 엔진이 할당되지 않았습니다. 디버그 중이라면 name을 채우시고, 실 개발 중이라면 생성 코드 뒤에 Initialize를 해주세요");
            }
            else
            {
                if (entityName == "Normal")
                {
                    Vector2Int vector2Int = Vector2Int.FloorToInt((Vector2)transform.position);
                    Engine_Normal normalEngine = new Engine_Normal(vector2Int);
                    Initialize(normalEngine);
                }
                else
                {
                    Debug.LogError("잘못된 string 값을 입력해서 디버그용 엔진 class를 불러올 수 없습니다.");
                }
            }
        }

        private void OnDestroy()
        {
            if (engineEntity != null)
            {
                engineEntity.OnEngineRequestActivated -= HandleCoroutineRequest;
                engineEntity.OnActivated -= HandleEngineActivated;
                engineEntity.OnUpdated -= HandleUpdated;
            }
        }



        #endregion
    }
}
