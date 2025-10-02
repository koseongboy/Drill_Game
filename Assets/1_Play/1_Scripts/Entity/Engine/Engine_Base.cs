using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DrillGame.Data;
using DrillGame.Managers;
using DrillGame.Components;
using DrillGame.Components.Engine;


namespace DrillGame.Entity.Engine
{
    public abstract class Engine_Base : Entity_Base
    {
        #region Fields & Properties
        protected float waitDelay;

        protected Action OnActivated;


        #endregion

        #region Singleton & initialization
        public Engine_Base(ComponentBaseController baseController, Vector2Int position) : base(baseController, position) 
        {
            
        }

        protected override void Initialize(ComponentBaseController baseController, Vector2Int position)
        {
            base.Initialize(baseController, position);
            Engine_Core.Instance.AddEngine(this);
            BoardManager.Instance.RegisterEngine(this);
            Debug.Log($"{entityName} 엔진 생성 및 코어, BoardManager register.");



            UpdateWaitDelay();
            UpDateEngineObjectInspector();
        }
        #endregion

        #region getters & setters
        
        public override void UpdatePosition(Vector2Int newPosition)
        {
            base.UpdatePosition(newPosition);

            UpdateWaitDelay();
            UpDateEngineObjectInspector();
        }

        async public void SetTileFormation(string id)
        {
            Engine_Data engineData;
            engineData = await Engine_Data.CreateAsync();
            Tuple<int, int>[] coordinates = engineData.GetCoordinate(id);
            foreach (var coordinate in coordinates)
            {
                TileFormation.Add(new Vector2Int(coordinate.Item1, coordinate.Item2));
            }
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
            Debug.Log($"{entityName} 대기 시간 업데이트: {waitDelay}초");
        }

        public void RequestActivate()
        {
            Debug.Log($"{entityName} 활성화 요청. 대기 시간: {waitDelay}초");
            EngineController engineController = baseController as EngineController;
            engineController.ActivateEngineWithDelay(waitDelay, ActivateEngine);
        }

        #endregion

        #region protected methods


        protected virtual void ActivateEngine()
        {
            Debug.Log($"{entityName} 엔진 활성화!");
            // 처리 로직

            List<Vector2Int> affectedPositions = GetAllPositions();
            BoardManager.Instance.EngineAction(affectedPositions, entityName);



            // baseController : monoBehaviour 의 함수 호출 현재 (HandleEngineActivated)
            OnActivated?.Invoke();
        }
        #endregion
        #region private methods
        // 디버그용 유니티 인스펙터 업데이트
        private void UpDateEngineObjectInspector()
        {
            // 이름은 생성시 고정
            baseController.Position = position;

            EngineController engineController = baseController as EngineController;
            engineController.engineWaitDelay = waitDelay;
        }


        #endregion
    }
}
