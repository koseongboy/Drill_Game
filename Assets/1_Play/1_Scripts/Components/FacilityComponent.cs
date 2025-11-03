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
using DrillGame.View.Helper;

namespace DrillGame.View.Facility
{
    public class FacilityComponent : MonoBehaviour, IPointerClickHandler, IDrillGameObjectInit
    {
        #region Fields & Properties
        [SerializeField]
        private Vector2Int debugPosition; // -> 이거 디버깅 이후에도 유지가능할거 같지 않나? 포메이션은 static 한 data니까
        [SerializeField]
        List<Vector2Int> debugFormation = new();
        [SerializeField]
        string debugActionClassName = "HelloFacilityAction";

        private FacilityPresenter presenter;

        public Action OnClickFacilityDetail { get; set; }

        #endregion

        #region Singleton & initialization
        public void Initialize(Vector2Int startPosition)
        {

            // for Test 후일 팩토리 패턴으로 분리 필요
            if(debugActionClassName != "HelloFacilityAction")
            {
                Debug.LogWarning("현재는 HelloFacilityAction만 지원합니다. 기본값으로 설정합니다.");
                debugActionClassName = "HelloFacilityAction";
            }

            IFacilityAction debugFormationAction = new HelloFacilityAction();
            FacilityEntity facilityEntity = new FacilityEntity(startPosition, debugFormation, debugFormationAction);
            presenter = new FacilityPresenter(this, facilityEntity);

            OnClickFacilityDetail = () => {
                presenter.RequestFacilityDetail();
                // 확장성을 위해 람다식 사용
            };

            // set debug position
            debugPosition = startPosition;
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void RunFacilityComponent(int intensity)
        {
            // 임시 그래픽 액션 실행
            TempGraphicAction(intensity);
        }
        

        #endregion

        #region private methods
        private void TempGraphicAction(int intensity)
        {
            // 임시 그래픽 액션
            transform.DOKill(true);
            transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0) * intensity, 0.1f, 10, 1);
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
                if(debugActionClassName != "HelloFacilityAction")
                {
                    Debug.LogWarning("현재는 HelloFacilityAction만 지원합니다. 기본값으로 설정합니다.");
                    debugActionClassName = "HelloFacilityAction";
                }
                Initialize(debugPosition);
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("FacilityComponent clicked : UI 필요해요");
        }
        #endregion

    }
}
