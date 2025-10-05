using UnityEngine;

using DrillGame.View.Engine;
using DrillGame.Core.Engine;


namespace DrillGame.Core.Presenter
{
    public class EnginePresenter
    {
        #region Fields & Properties
        private readonly EngineComponent engineComponent;
        private readonly EngineEntity engineEntity;

        #endregion

        #region Singleton & initialization
        public EnginePresenter(EngineComponent engineComponent, EngineEntity engineEntity)
        {
            this.engineComponent = engineComponent;
            this.engineEntity = engineEntity;

            engineEntity.OnEngineActivated += OnEngineEntityActivated;
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void Dispose()
        {
            engineEntity.OnEngineActivated -= OnEngineEntityActivated;
        }
        public void RequestEngineDetail()
        {
            engineEntity.ShowEngineInfo();

        }
        #endregion

        #region private methods
        private void OnEngineEntityActivated()
        {
            engineComponent.RunEngineComponent();

        }
        #endregion

    }
}
