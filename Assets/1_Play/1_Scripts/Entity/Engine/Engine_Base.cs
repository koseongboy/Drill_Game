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
        public event Action<IEnumerator, Action> OnEngineRequestActivated;
        public event Action OnUpdated;
        public event Action OnActivated;

        public float waitDelay { get; protected set; }






        #endregion

        #region Singleton & initialization
        public Engine_Base(Vector2Int position) : base(position) 
        {
            
        }

        protected override void Initialize(Vector2Int position)
        {
            base.Initialize(position);
            Engine_Core.Instance.AddEngine(this);
            BoardManager.Instance.RegisterEngine(this);
            Debug.Log($"{entityName} 엔진 생성 및 코어, BoardManager register.");



            UpdateWaitDelay();
            // issue : 모노비헤비어 객체 생성후 initialize 전에 data 객체가 먼저 init 되기에 이벤트 체인이 되지 않음
            //OnUpdated?.Invoke();
        }
        #endregion

        #region getters & setters

        public override void UpdatePosition(Vector2Int newPosition)
        {
            base.UpdatePosition(newPosition);

            UpdateWaitDelay();
            OnUpdated?.Invoke();
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
        

        public void UpdateWaitDelay()
        {
            waitDelay = Vector2Int.Distance(Engine_Core.Instance.position, position) * 0.1f;
            Debug.Log($"{entityName} 대기 시간 업데이트: {waitDelay}초");
        }

        public void RequestActivate()
        {
            IEnumerator enumerator = WaitAndActivate(waitDelay);
            Action action = ActivateEngine;

            
            OnEngineRequestActivated?.Invoke(enumerator, action);
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
        

        private IEnumerator WaitAndActivate(float delay)
        {
            yield return new WaitForSeconds(delay);
        }

        #endregion
    }

    
}
