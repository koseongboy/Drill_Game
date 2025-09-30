using DG.Tweening;
using DrillGame.Entity.Engine;
using System;
using System.Collections;
using UnityEngine;

namespace DrillGame.Components.Engine
{
    public class EngineController : MonoBehaviour
    {
        #region Fields & Properties
        private Engine_Base engine;

        [SerializeField]
        [Tooltip("debug 용도로 엔진의 이름을 직접 작성할 수 있습니다. 실 사용시 비워두세요")]
        private string engineName;
        [ReadOnly]
        [SerializeField]
        public Vector2Int enginePosition;
        [ReadOnly]
        [SerializeField]
        public float engineWaitDelay;

        #endregion

        #region Singleton & initialization
        public void Initialize(Engine_Base engine)
        {
            this.engine = engine;
            engineName = engine.GetType().Name;
            engine.OnActivatedEvent += HandleEngineActivated;
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void ActivateEngineWithDelay(float delay, Action action)
        {
            if(delay > 0)
            {
                StartCoroutine(WaitAndActivate(delay, action));
            }
            else
            {
                action?.Invoke();
            }
        }

        #endregion

        #region private methods
        private void HandleEngineActivated()
        {
            Debug.Log($"mono - {engineName} 엔진이 active 되었습니다.");
            UpDateEngineObject();
        }

        private IEnumerator WaitAndActivate(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        private void UpDateEngineObject()
        {
             TempGraphicAction();
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
            if (engineName == null)
            {
                Debug.LogWarning("EngineController에 엔진이 할당되지 않았습니다. 디버그 중이라면 name을 채우시고, 실 개발 중이라면 생성 코드 뒤에 Initialize를 해주세요");
            }
            else
            {
                if (engineName == "Normal")
                {
                    Vector2Int vector2Int = Vector2Int.FloorToInt((Vector2)transform.position);
                    Engine_Normal normalEngine = new Engine_Normal(this, vector2Int);
                    Initialize(normalEngine);
                }
                else
                {
                    Debug.LogError("잘못된 string 값을 입력해서 디버그용 엔진 class를 불러올 수 없습니다.");
                }
            }
        }

        


        #endregion
    }
}
