using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.Core.Ground
{
    public class GroundEntity
    {
        #region Fields & Properties
        public int Depth { get; private set; }
        public int MaxHp { get; private set; }
        public int CurrentHp { get; private set; }
        public bool IsDestroyed => CurrentHp <= 0;

        public List<string> DropItems { get; private set; }
        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        public void SetInformation(int depth, int current_hp, int max_hp, List<string> dropItems)
        {
            Depth = depth;
            CurrentHp = current_hp;
            MaxHp = max_hp;
            DropItems = dropItems;
        }
        public int GetNextDepth() { return Depth + 1; }
        #endregion

        #region public methods
        public bool GiveDamage(int damage)
        {
            CurrentHp -= damage;
            return IsDestroyed;
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}