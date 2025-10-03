using DrillGame.Components;
using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.Entity
{
    public abstract class Entity_Base
    {
        #region Fields & Properties
        protected string entityName;


        public Vector2Int position { get; protected set; }
        protected List<Vector2Int> TileFormation = new() { new Vector2Int(0, 0) }; // default 1x1

        

        #endregion

        #region Singleton & initialization
        public Entity_Base(Vector2Int position)
        {
            entityName = GetType().Name;

            this.position = position;
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

        

        public string GetEntityName()
        {
            return entityName;
        }

        public virtual void UpdatePosition(Vector2Int newPosition)
        {
            position = newPosition;
            Debug.Log($"{entityName} 위치 변경 {position}");

        }
        #endregion

        #region public methods

        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}
