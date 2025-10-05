using DG.Tweening;
using DrillGame.Core.Engine;
using DrillGame.Core.Facility;
using DrillGame.Core.Presenter;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace DrillGame.View.Facility
{
    public class FacilityComponent : MonoBehaviour, IPointerClickHandler
    {
        #region Fields & Properties
        [SerializeField]
        private Vector2Int debugPosition;
        [SerializeField]
        List<Vector2Int> debugFormation = new();

        private FacilityPresenter presenter;

        public Action OnClickFacilityDetail { get; set; }

        #endregion

        #region Singleton & initialization
        public void Initialize(FacilityEntity facilityEntity)
        {
            presenter = new FacilityPresenter(this, facilityEntity);

            OnClickFacilityDetail = () => {
                presenter.RequestFacilityDetail();
                // 확장성을 위해 람다식 사용
            };
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void RunFacilityComponent()
        {
            // 임시 그래픽 액션 실행
            TempGraphicAction();
        }
        #endregion

        #region private methods
        private void TempGraphicAction()
        {
            // 임시 그래픽 액션
            transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.2f, 10, 1);
        }
        #endregion

        #region Unity event methods
        private void Awake()
        {
            // init 사용을 권장

        }
        private void Start()
        {
            if (presenter == null)
            {
                Debug.LogWarning("씬에서 직접 FacilityComponent를 생성했습니다. 테스트용 기본 시설을 생성합니다.");
                Initialize(new FacilityEntity(debugPosition, debugFormation));
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
        #endregion

    }
}
