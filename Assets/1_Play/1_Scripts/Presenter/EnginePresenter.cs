using DrillGame.Core.Engine;
using DrillGame.Core.Facility;
using DrillGame.UI;
using DrillGame.View.Engine;
using UnityEngine;


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
            engineEntity.OnEngineDeleted += OnEngineEntityDeleted;
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void Dispose()
        {
            engineEntity.OnEngineActivated -= OnEngineEntityActivated;
            engineEntity.OnEngineDeleted -= OnEngineEntityDeleted;
        }
        public void RequestEngineDetail()
        {
            UILoader.Instance.ShowUI_EngineDetail(engineEntity);
        }
        #endregion

        #region private methods
        private void OnEngineEntityActivated()
        {
            engineComponent.RunEngineComponent();

        }

        private void OnEngineEntityDeleted()
        {
            engineComponent.DeleteEngineComponent();
            
        }
        #endregion

    }
}
