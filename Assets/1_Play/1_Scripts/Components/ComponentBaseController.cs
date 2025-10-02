using UnityEngine;

using DrillGame.Entity;

namespace DrillGame.Components
{
    public class ComponentBaseController : MonoBehaviour
    {
        #region Fields & Properties
        protected Entity_Base entity;

        [SerializeField]
        [Tooltip("debug 용도로 엔진의 이름을 직접 작성할 수 있습니다. 실 사용시 비워두세요")]
        protected string entityName;
        [ReadOnly]
        [SerializeField]    
        protected Vector2Int position;

        #endregion

        #region Singleton & initialization
        public virtual void Initialize(Entity_Base entity)
        {
            this.entity = entity;
            this.entityName = entity.GetType().Name;
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        #endregion

        #region protectd methods
        protected virtual void HandleUpdated()
        {
            this.position = entity.position;

        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}
