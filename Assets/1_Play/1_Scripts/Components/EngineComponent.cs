using DG.Tweening;
using DrillGame.Core.Engine;
using DrillGame.Core.Presenter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DrillGame.View.Engine
{
    public class EngineComponent : MonoBehaviour, IPointerClickHandler
    {
        #region Fields & Properties
        [SerializeField]
        private Vector2Int debugPosition;
        [SerializeField]
        List<Vector2Int> debugFormation = new();

        private EnginePresenter presenter;
        public Action OnClickEngineDetail { get; set; }


        // for temp graphic action
        private SpriteRenderer spriteRenderer;
        private Color originalColor;
        private Color flashColor = Color.yellow;
        private float flashDuration = 0.15f;

        #endregion

        #region Singleton & initialization
        public void Initialize(EngineEntity engineEntity)
        {
            presenter = new EnginePresenter(this, engineEntity);

            OnClickEngineDetail = () => {
                presenter.RequestEngineDetail();
                // 확장성을 위해 람다식 사용
            };

            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.material.color;
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        // 엔진 컴포넌트 관련 기능 실행
        public void RunEngineComponent()
        {
            // 임시 그래픽 액션 실행
            TempGraphicAction();
        }
        #endregion

        #region private methods
        private void TempGraphicAction()
        {
            // 임시 그래픽 액션 : 색깔을  잠깐 바꿨다가 원래대로
            spriteRenderer.material.DOColor(flashColor, flashDuration)
            // 2. 변경이 완료된 후 실행될 콜백 지정
            .OnComplete(() =>
            {
                // 콜백에서 원래 색상으로 복귀
                spriteRenderer.material.DOColor(originalColor, flashDuration);
            });



        }
        #endregion

        #region Unity event methods
        private void Awake()
        {
            // init 사용을 권장
            
        }

        private void Start()
        {
            if(presenter == null)
            {
                Debug.LogWarning("씬에서 직접 EngineComponent를 생성했습니다. 테스트용 기본 엔진을 생성합니다.");
                Initialize(new EngineEntity(debugPosition, debugFormation));
            }
        }

        private void OnDestroy()
        {
            presenter.Dispose();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickEngineDetail?.Invoke();
        }


        #endregion
    }
}
