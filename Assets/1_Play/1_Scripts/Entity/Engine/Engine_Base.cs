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
            Engine_Core.Instance.AddEngine(this);
            BoardManager.Instance.RegisterEngine(this);
            Debug.Log($"{entityName} ���� ���� �� �ھ�, BoardManager register.");



            UpdateWaitDelay();
            // issue : �������� ��ü ������ initialize ���� data ��ü�� ���� init �Ǳ⿡ �̺�Ʈ ü���� ���� ����
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
            List<Tuple<int, int>> coordinates = engineData.GetCoordinate(id);
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
            Debug.Log($"{entityName} ��� �ð� ������Ʈ: {waitDelay}��");
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
            Debug.Log($"{entityName} ���� Ȱ��ȭ!");
            // ó�� ����

            List<Vector2Int> affectedPositions = GetAllPositions();
            BoardManager.Instance.EngineAction(affectedPositions, entityName);



            // baseController : monoBehaviour �� �Լ� ȣ�� ���� (HandleEngineActivated)
            OnActivated?.Invoke();
        }
        #endregion
        #region private methods
        

        private IEnumerator WaitAndActivate(float delay)
        {
            yield return new WaitForSeconds(delay);
        }

        #endregion
    }

    
}
