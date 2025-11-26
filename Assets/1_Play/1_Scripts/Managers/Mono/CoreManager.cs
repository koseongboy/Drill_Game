using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DrillGame
{
    public class CoreManager : MonoBehaviour
    {
        #region Fields & Properties
        private Vector2Int CORE_POSITION = new Vector2Int(-1, 0);
        private List<Vector2Int> CORE_FORMATIONS = new List<Vector2Int>()
        {
            // 3*3
            new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1),
            new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0),
            new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
        };

        private int coreLevel = 1;

        #endregion

        #region Singleton & initialization
        public static CoreManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        #endregion

        #region getters & setters
        public List<Vector2Int> GetCoreFormations()
        {
            List<Vector2Int> absolutePositions = new List<Vector2Int>();
            foreach (var formation in CORE_FORMATIONS)
            {
                absolutePositions.Add(CORE_POSITION + formation);
            }
            return absolutePositions;
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
