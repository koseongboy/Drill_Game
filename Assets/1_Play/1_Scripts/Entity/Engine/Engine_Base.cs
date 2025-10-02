using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DrillGame.Components.Engine;
using DrillGame.Data;
using DrillGame.Managers;


namespace DrillGame.Entity.Engine
{
    public abstract class Engine_Base : Entity_Base
    {
        #region Fields & Properties
        protected float waitDelay;

        protected EngineController engineController;

        protected Action OnActivated;


        #endregion

        #region Singleton & initialization
        public Engine_Base(EngineController engineController, Vector2Int position)
        {
            Initialize(engineController, position);
        }

        private void Initialize(EngineController engineController, Vector2Int position)
        {
            entityName = GetType().Name;
            Engine_Core.Instance.AddEngine(this);
            BoardManager.Instance.RegisterEngine(this);

            Debug.Log($"{entityName} 생성 및 코어, BoardManager register.");

            this.engineController = engineController;
            this.position = position;
            UpdateWaitDelay();

            UpDateEngineObjectInspector();
        }
        #endregion

        #region getters & setters
        public List<Vector2Int> GetAllPositions()
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            foreach (var offset in TileFormation)
            {
                positions.Add(position + offset);
            }
            return positions;
        }
        public void UpdatePosition(Vector2Int newPosition)
        {
            position = newPosition;
            Debug.Log($"{entityName} 위치 변경 {position}");
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
            engineController.ActivateEngineWithDelay(waitDelay, ActivateEngine);
        }

        #endregion

        #region protected methods


        protected virtual void ActivateEngine()
        {
            Debug.Log($"{entityName} 엔진 활성화!");
            // 처리 로직

            BoardManager.Instance.EngineAction(TileFormation, entityName);



            // engineController : monoBehaviour 의 함수 호출 현재 (HandleEngineActivated)
            OnActivated?.Invoke();
        }
        #endregion
        #region private methods
        // 디버그용 유니티 인스펙터 업데이트
        private void UpDateEngineObjectInspector()
        {
            // 이름은 생성시 고정
            engineController.Position = position;
            engineController.engineWaitDelay = waitDelay;
        }


        #endregion
    }
}
