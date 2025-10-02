using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.Entity
{
    public abstract class Entity_Base
    {
        #region Fields & Properties
        protected string entityName;


        protected Vector2Int position;
        protected List<Vector2Int> TileFormation = new() { new Vector2Int(0, 0) }; // default 1x1

        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}
