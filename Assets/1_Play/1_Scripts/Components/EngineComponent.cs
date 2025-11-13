using DG.Tweening;
using DrillGame.Core.Engine;
using DrillGame.Core.Presenter;
using DrillGame.View.Helper;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DrillGame.View.Engine
{
    public class EngineComponent : MonoBehaviour, IPointerClickHandler, IDrillGameObjectInit, IDrillGameDefaultGrapic, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields & Properties
        [SerializeField]
        private Vector2Int debugPosition;   // -> 이거 디버깅 이후에도 유지가능할거 같지 않나? 포메이션은 static 한 data니까
        [SerializeField]
        private List<Vector2Int> debugFormation = new();
        [SerializeField]
        private string engineType = "BasicEngine";

        private EnginePresenter presenter;
        public Action OnClickEngineDetail { get; set; }


        // for temp graphic action
        private SpriteRenderer spriteRenderer;
        private Color originalColor;
        private Color flashColor = Color.yellow;
        private float flashDuration = 0.15f;

        private Color onMouseColor = Color.cyan;

        #endregion

        #region Singleton & initialization
        public void Initialize(Vector2Int startPosition)
        {
            // for Test 후일 팩토리 패턴으로 분리 필요 -> 근데 아직 엔진 행동 패턴이 없는..
            if (engineType != "BasicEngine")
            {
                Debug.LogWarning("현재는 BasicEngine만 지원합니다. 기본값으로 설정합니다.");
                engineType = "BasicEngine";
            }

            EngineEntity engineEntity = new EngineEntity(startPosition, debugFormation);

            presenter = new EnginePresenter(this, engineEntity);

            OnClickEngineDetail = () => {
                presenter.RequestEngineDetail();
                // 확장성을 위해 람다식 사용
            };

            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.material.color;


            // set debug position
            debugPosition = startPosition;
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

        public void DeleteEngineComponent()
        {
            // 엔진 컴포넌트 삭제 처리
            Destroy(this.gameObject);
            // onDestroy에서 presenter.Dispose() 호출
        }

        public void ChosenGraphic()
        {
            spriteRenderer.material.color = Color.green;
        }

        public void DefaultGraphic()
        {
            spriteRenderer.material.color = originalColor;
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
                Initialize(debugPosition);
            }
        }

        private void OnDestroy()
        {
            presenter.Dispose();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // 지금 테스트 용도라서 삭제를 여기다가 걸어두었는데 그러면 배치하자마자 눌려서 가운데 클릭으로 바꿨어요
            if(eventData.button != PointerEventData.InputButton.Middle)
            {
                return; 
            }
            OnClickEngineDetail?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = onMouseColor;
        }

        // IPointerExitHandler의 필수 메서드 구현
        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = originalColor;
        }



        #endregion
    }
}
