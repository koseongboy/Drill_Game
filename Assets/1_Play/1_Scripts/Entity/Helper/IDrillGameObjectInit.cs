using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.View.Helper
{
    public interface IDrillGameObjectInit
    {
        void Initialize(Vector2Int startPosition);

    }
}
