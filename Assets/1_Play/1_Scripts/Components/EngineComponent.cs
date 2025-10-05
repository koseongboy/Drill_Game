using UnityEngine;
using System;
using UnityEngine.EventSystems;

using DrillGame.Core.Presenter;
using DrillGame.Core.Engine;
using DG.Tweening;

namespace DrillGame.View.Engine
{
    public class EngineComponent : MonoBehaviour, IPointerClickHandler
    {
        #region Fields & Properties
        [SerializeField]
        private Vector2Int debugPosition;

        private EnginePresenter presenter;
        public Action OnClickEngineDetail { get; set; }




        #endregion

        #region Singleton & initialization
        public void Initialize(EngineEntity engineEntity)
        {
            presenter = new EnginePresenter(this, engineEntity);

            OnClickEngineDetail = () => {
                presenter.RequestEngineDetail();
                // 확장성을 위해 람다식 사용
            };
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
                       // 임시 그래픽 액션
            transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.2f, 10, 1);
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
                Initialize(new EngineEntity(debugPosition));
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
