using DrillGame.Components.Engine;
using DrillGame.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DrillGame.Entity.Engine
{
    public abstract class Engine_Base
    {
        #region Fields & Properties
        [ReadOnly]
        [SerializeField]
        protected string engineName;
        [ReadOnly]
        [SerializeField]
        protected float waitDelay;

        protected EngineController engineController;

        protected Action OnActivated;

        private List<Vector2Int> TileFormation;

        public Vector2Int position { get; private set; }
        #endregion

        #region Singleton & initialization
        public Engine_Base(EngineController engineController, Vector2Int position)
        {
            Initialize(engineController, position);
        }

        private void Initialize(EngineController engineController, Vector2Int position)
        {
            engineName = GetType().Name;
            Engine_Core.Instance.AddEngine(this);
            FlowManager.Instance.RegisterEngine(this);

            Debug.Log($"{engineName} 생성 및 코어, FlowController register.");

            this.engineController = engineController;
            this.position = position;
            UpdateWaitDelay();

            UpDateEngineObjectInspector();
        }
        #endregion

        #region getters & setters
        public void UpdatePosition(Vector2Int newPosition)
        {
            position = newPosition;
            Debug.Log($"{engineName} 위치 변경 {position}");
            UpdateWaitDelay();

            UpDateEngineObjectInspector();
        }

        #endregion

        #region public methods
        public event Action OnActivatedEvent
        {
            add { OnActivated += value; }
            remove { OnActivated -= value; }
        }

        public void UpdateWaitDelay()
        {
            waitDelay = Vector2Int.Distance(Engine_Core.Instance.position, position) * 0.1f;
            Debug.Log($"{engineName} 대기 시간 업데이트: {waitDelay}초");
        }

        public void RequestActivate()
        {
            Debug.Log($"{engineName} 활성화 요청. 대기 시간: {waitDelay}초");
            engineController.ActivateEngineWithDelay(waitDelay, ActivateEngine);
        }


        // todo(고성): 타일 포메이션 설정
        public void SetTileFormation()
        {
            TileFormation = new List<Vector2Int>();
        }
        #endregion

        #region protected methods


        protected virtual void ActivateEngine()
        {
            Debug.Log($"{engineName} 엔진 활성화!");
            // 처리 로직

            FlowManager.Instance.EngineAction(TileFormation, engineName);



            // engineController : monoBehaviour 의 함수 호출 현재 (HandleEngineActivated)
            OnActivated?.Invoke();
        }
        #endregion
        #region private methods
        // 디버그용 유니티 인스펙터 업데이트
        private void UpDateEngineObjectInspector()
        {
            // 이름은 생성시 고정
            engineController.enginePosition = position;
            engineController.engineWaitDelay = waitDelay;
        }


        #endregion
    }
}
