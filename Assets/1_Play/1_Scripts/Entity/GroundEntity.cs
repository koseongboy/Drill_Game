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
        #endregion

        #region public methods
        public void GiveDamage(int damage)
        {
            CurrentHp -= damage;
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}