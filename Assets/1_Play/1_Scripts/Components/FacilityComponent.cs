using DG.Tweening;
using DrillGame.Core.Facility;
using DrillGame.Core.Presenter;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrillGame.View.Helper;
using UnityEngine.Serialization;
using DrillGame.Managers;

namespace DrillGame.View.Facility
{
    public class FacilityComponent : MonoBehaviour, IPointerClickHandler, IDrillGameObjectInit, IDrillGameDefaultGrapic, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields & Properties


        [SerializeField] 
        [ReadOnly] 
        private Vector2Int debugPosition; // -> 이거 디버깅 이후에도 유지가능할거 같지 않나? 포메이션은 static 한 data니까
        private Vector2 pivot;

        

        private FacilityPresenter presenter;

        public Action OnClickFacilityDetail { get; set; }


        private Facility_Data_ data;
        // graphic action 관련 임시 필드
        private SpriteRenderer spriteRenderer;
        private BoxCollider2D boxCollider;
        private Color originalColor;

        private Color onMouseColor = Color.cyan;

        // graphic action 관련
        private ParticleSystem runEffect;

        #endregion

        #region Singleton & initialization
        public void Initialize(Vector2Int startPosition, int itemId = 0, int entityId = 0)
        {
            data = ScriptableObjectManager.Instance.GetData<Facility_Data_>(entityId);
            string fullEntityClassName = "DrillGame.Core.Facility." + data.EntityClassName;
            Type type = Type.GetType(fullEntityClassName);
            Debug.Log("Facility Action Type : " + type);
            if (type == null)
            {
                Debug.LogError($"엔티티 클래스 '{fullEntityClassName}'는 없는 엔티티입니다.");
                type = typeof(FacilityEntity);
            }

            object[] parameters = new object[] { startPosition, 0, itemId, entityId };
            FacilityEntity facilityEntity = Activator.CreateInstance(type, parameters) as FacilityEntity;
            presenter = new FacilityPresenter(this, facilityEntity);

            OnClickFacilityDetail = () => {
                presenter.RequestFacilityDetail();
                // 확장성을 위해 람다식 사용
            };
            

            //여기서부턴 그래픽
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
            //pivot 설정
            pivot = GetPivot(data.GetLength());
            //스프라이트 적용하기 - 동시성
            ChangeIcon();

            originalColor = spriteRenderer.material.color;

            runEffect = GetComponent<ParticleSystem>();

            //Collider 크기 조절
            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
            boxCollider.size = spriteSize;
            boxCollider.offset = spriteRenderer.sprite.bounds.center;

            // set debug position
            debugPosition = startPosition;
            transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void RunFacilityComponent(int intensity)
        {
            // 임시 그래픽 액션 실행
            TempGraphicAction(intensity);
            // 이펙트 재생
            Debug.Log("Facility run effect play");
            if (runEffect != null)
            {
                runEffect.Play();
            }
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

        private Vector2 GetPivot(Vector2Int length)
        {
            float newx, newy;
            if (length.x % 2 == 0)
            {
                newx = 1f / (length.x * 2); //짝수 개면 1/(길이*2)
            }
            else
            {
                newx = 0.5f; //홀수 개면 중앙이 중심
            }
            if (length.y % 2 == 0)
            {
                newy = 1f / (length.y * 2);
            }
            else
            {
                newy = 0.5f;
            }

            return new Vector2(newx, newy);
        }

        private async void ChangeIcon()
        {
            try
            {
                var originSprite = await SpriteLoader.Instance.LoadSprite(data.Icon);
                
                Texture2D texture = originSprite.texture;
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                float pixelsPerUnit = texture.width / data.GetLength().x; // 가로 길이를 기준으로 픽셀 퍼 유닛 설정
                Sprite facilitySprite = Sprite.Create(texture, rect, pivot, pixelsPerUnit);
                spriteRenderer.sprite = facilitySprite;
            }
            catch
            {
                Debug.LogError("Facility sprite load failed : " + data.Icon);
            }
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

            if (GameManager.Instance.isInBatchMode())
            {
                Debug.Log("배치 모드 중에는 시설 상세 정보를 볼 수 없습니다.");
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
