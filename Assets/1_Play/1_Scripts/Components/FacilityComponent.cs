using DG.Tweening;
using DrillGame.Core.Facility;
using DrillGame.Core.Presenter;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DrillGame.View.Helper;

namespace DrillGame.View.Facility
{
    public class FacilityComponent : MonoBehaviour, IPointerClickHandler, IDrillGameObjectInit, IDrillGameDefaultGrapic, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields & Properties

        //내부 정보

        [SerializeField]
        public int debugId = 101011;
        [SerializeField]
        private Vector2Int debugPosition; // -> 이거 디버깅 이후에도 유지가능할거 같지 않나? 포메이션은 static 한 data니까

        [SerializeField]
        public string EntityClassName = "FacilityEntity";
        

        private FacilityPresenter presenter;

        public Action OnClickFacilityDetail { get; set; }

        // graphic action 관련 임시 필드
        private SpriteRenderer spriteRenderer;
        private Color originalColor;

        private Color onMouseColor = Color.cyan;

        #endregion

        #region Singleton & initialization
        public void Initialize(Vector2Int startPosition, int itemId = 0, int entityId = 0)
        {
            // 각 장치 속성에 맞는 엔티티 동적 생성
            string fullEntityClassName = "DrillGame.Core.Facility." + EntityClassName;
            Type type = System.Type.GetType(fullEntityClassName);
            Debug.Log("Facility Action Type : " + type);
            if (type == null)
            {
                Debug.LogError($"Facility action class '{fullEntityClassName}' not found. Using default action.");
                type = typeof(FacilityEntity);
            }
            object[] parameters = new object[] { startPosition, 0, itemId, debugId };
            FacilityEntity facilityEntity = Activator.CreateInstance(type, parameters) as FacilityEntity;
            presenter = new FacilityPresenter(this, facilityEntity);

            OnClickFacilityDetail = () => {
                presenter.RequestFacilityDetail();
                // 확장성을 위해 람다식 사용
            };

            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.material.color;

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

        public void DeleteFacilityComponent()
        {
            Destroy(this.gameObject);
        }

        public void ChosenGraphic()
        {
            spriteRenderer.material.color = Color.green;
        }

        public void DefaultGraphic()
        {
            spriteRenderer.material.color = originalColor;
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
                Initialize(debugPosition);
            }
        }
        private void OnDestroy()
        {
            presenter.Dispose();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            OnClickFacilityDetail?.Invoke();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = onMouseColor;
        }

        // IPointerExitHandler의 필수 메서드 구현
        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = originalColor;
        }
        #endregion

    }
}
