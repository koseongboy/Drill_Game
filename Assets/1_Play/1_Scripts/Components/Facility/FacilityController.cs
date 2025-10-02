using DG.Tweening;
using DrillGame.Entity;
using DrillGame.Entity.Facility;
using DrillGame.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DrillGame.Components.Facility
{
    public class FacilityController : ComponentBaseController, IPointerClickHandler
    {
        #region Fields & Properties
        private Facility_Base facilityEntity;

        #endregion

        #region Singleton & initialization
        public override void Initialize(Entity_Base facility)
        {
            base.Initialize(facility);

            

            facilityEntity = entity as Facility_Base;
            facilityEntity.OnActivated += HandleFacilityActivated;

            facilityEntity.OnUpdated += HandleUpdated;

            HandleUpdated(); // 초기값 세팅
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        private void UpDateFacilityObject()
        {
            Debug.Log($"mono - {entityName} 시설 오브젝트가 업데이트 되었습니다.");
            TempGrapicAction();
        }
        #endregion

        #region private methods
        private void HandleFacilityActivated()
        {
            UpDateFacilityObject();
        }

        private void TempGrapicAction()
        {
            // Vector3.one * 0.15f는 모든 축으로 0.15만큼 펀치한다는 의미입니다.
            transform.DOPunchScale(
                punch: Vector3.one * 0.2f, // 현재 크기에서 20% 커짐
                duration: 0.15f,             // 0.15초 동안 커졌다 돌아옴
                vibrato: 1,                 // 한 번만 튀어나왔다 돌아오도록 설정
                elasticity: 1               // 탄성력 설정 (1이 일반적으로 부드러움)
            );
        }

        protected override void HandleUpdated()
        {
            base.HandleUpdated();
        }
        #endregion

        #region Unity event methods
        private void Awake()
        {
            if (entityName == null)
            {
                Debug.LogError("FacilityController에 시설이 할당되지 않았습니다. 디버그 중이라면 이름을 작성하시고, Initialize 메서드를 호출하여 시설을 할당하세요.");

            }
            else
            {
                if (entityName == "Hello")
                {
                    Vector2Int vector2Int = Vector2Int.FloorToInt((Vector2)transform.position);
                    Facility_Hello normalFacility = new Facility_Hello(vector2Int);
                    Initialize(normalFacility);
                }
                else
                {
                    Debug.LogError("잘못된 string 값을 입력해서 디버그용 시설 class를 불러올 수 없습니다");
                }
            }
        }

        private void OnDestroy()
        {
            if (facilityEntity != null)
            {
                facilityEntity.OnActivated -= HandleFacilityActivated;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"{entityName} 시설이 클릭되었습니다.");
            UILoader.Instance.ShowUI(facilityEntity.GetFacilityUIName());
        }
        #endregion
    }
}
