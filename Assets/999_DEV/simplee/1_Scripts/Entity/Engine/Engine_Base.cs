using System;
using UnityEngine;

namespace DrillGame.Entity.Engine
{
    public abstract class Engine_Base
    {
        protected Action OnActivated;

        public event Action OnActivatedEvent
        {
            add { OnActivated += value; }
            remove { OnActivated -= value; }
        }
        public abstract void Activate();
        public Vector2 position { get; private set; }


    }
}
