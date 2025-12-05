using DG.Tweening;
using DrillGame.Core.Engine;
using DrillGame.Core.Presenter;
using DrillGame.Managers;
using DrillGame.View.Helper;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DrillGame.View.Engine
{
    public class EngineComponent : MonoBehaviour, IPointerClickHandler, IDrillGameObjectInit, IDrillGameDefaultGrapic, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields & Properties
        [SerializeField]
        [ReadOnly]
        private Vector2Int debugPosition;   // -> 이거 디버깅 이후에도 유지가능할거 같지 않나? 포메이션은 static 한 data니까
        [SerializeField]
        [ReadOnly]
        private string engineType;
        [SerializeField]
        Sprite MainTile;
        [SerializeField]
        Sprite NormalTile;
        List<Vector2Int> formations;


        private EnginePresenter presenter;
        public Action OnClickEngineDetail { get; set; }

        Engine_Data_ data;

        public const int CORE_ENTITY_ID = 210001;

        // for temp graphic action
        private SpriteRenderer spriteRenderer;
        
        private Color originalColor;
        private Color flashColor = Color.white;
        private float flashDuration = 0.15f;

        private Color onMouseColor = Color.cyan;

        private WireComponent wireComponent;

        #endregion

        #region Singleton & initialization
        public void InitializeCore(Vector2Int startPosition)
        {
            EngineEntity engineEntity = new EngineEntity(startPosition, new List<Vector2Int>(), 0, CORE_ENTITY_ID, "Core");

            presenter = new EnginePresenter(this, engineEntity);

            OnClickEngineDetail = () => {
                presenter.RequestEngineDetail();
                // 확장성을 위해 람다식 사용
            };

            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.material.color;


            // set debug position
            debugPosition = startPosition;
        }

        public void Initialize(Vector2Int startPosition, int itemId=0, int entityId=0)
        {
            BoxCollider2D existingCollider = GetComponent<BoxCollider2D>();
            if (existingCollider != null) Destroy(existingCollider);
            data = ScriptableObjectManager.Instance.GetData<Engine_Data_>(entityId);
            engineType = data.Type;
            formations = data.GetCoordinates();
            List<Vector2Int> entityFormations = new();
            foreach(var coord in data.GetCoordinates()) entityFormations.Add(coord - data.GetMainCoordinate());
            EngineEntity engineEntity = new EngineEntity(startPosition, entityFormations, itemId, entityId, engineType);

            presenter = new EnginePresenter(this, engineEntity);

            OnClickEngineDetail = () => {
                presenter.RequestEngineDetail();
                // 확장성을 위해 람다식 사용
            };

            spriteRenderer = GetComponent<SpriteRenderer>();

            if(entityId != CORE_ENTITY_ID)
            {
                wireComponent = this.gameObject.AddComponent<WireComponent>();
                wireComponent.SetWire(transform.position, CoreManager.Instance.GetCoreWorldPosition());
            }

            // 스프라이트 설정
            int finalWidth = data.GetLength().x * (int)NormalTile.rect.width;
            int finalHeight = data.GetLength().y * (int)NormalTile.rect.height;
            Texture2D texture = new Texture2D(finalWidth, finalHeight, TextureFormat.RGBA32, false);
            Color[] clearPixels = new Color[finalWidth * finalHeight];
            for (int i = 0; i < clearPixels.Length; i++) clearPixels[i] = new Color(0f, 0f, 0f, 0f);
            texture.SetPixels(clearPixels);
            texture.Apply();
            foreach(var formation in formations)
            {
                BoxCollider2D tileCollider = gameObject.AddComponent<BoxCollider2D>();
                tileCollider.size = new Vector2(1f, 1f);
                int xOffset = formation.x * (int)NormalTile.rect.width;
                int yOffset = formation.y * (int)NormalTile.rect.height;
                Vector2 worldUnitPos = new Vector2(xOffset, yOffset);
                tileCollider.offset = new Vector2(formation.x - data.GetMainCoordinate().x, formation.y - data.GetMainCoordinate().y);
                if(formation == data.GetMainCoordinate())
                {
                    // MainTile 그리기
                    Color[] mainPixels = MainTile.texture.GetPixels((int)MainTile.rect.x, (int)MainTile.rect.y, (int)MainTile.rect.width, (int)MainTile.rect.height);
                    texture.SetPixels(xOffset, yOffset, (int)MainTile.rect.width, (int)MainTile.rect.height, mainPixels);
                }
                else
                {
                    // NormalTile 그리기
                    Color[] normalPixels = NormalTile.texture.GetPixels((int)NormalTile.rect.x, (int)NormalTile.rect.y, (int)NormalTile.rect.width, (int)NormalTile.rect.height);
                    texture.SetPixels(xOffset, yOffset, (int)NormalTile.rect.width, (int)NormalTile.rect.height, normalPixels);
                }
            }
            texture.Apply();
            // 텍스쳐, 크기, 피벗, ppu
            Sprite combinedSprite = Sprite.Create(texture, new Rect(0, 0, finalWidth, finalHeight), GetPivot(data.GetMainCoordinate(), data.GetLength()), (int)NormalTile.rect.width);

            spriteRenderer.sprite = combinedSprite;
    
            if (ColorUtility.TryParseHtmlString(data.Type, out originalColor))
            {
                // 성공: data.Type이 유효한 Hex 코드 ("#RRGGBB" 또는 "#RRGGBBAA")인 경우
                spriteRenderer.color = originalColor;
                Debug.Log($"색상 Hex Code '{data.Type}'이(가) 성공적으로 적용되었습니다.");
            }
            else
            {
                // 실패: data.Type이 유효한 Hex 코드가 아닌 경우
                
                // 2. data.Type을 색상 이름으로 간주하고 사용자 정의 색상 매핑 시도
                
                // 예: data.Type이 "STONE"일 때 회색을 적용하도록 설정
                Color fallbackColor;
                
                if (TryGetDefinedColor(data.Type, out fallbackColor))
                {
                    originalColor = fallbackColor;
                    spriteRenderer.color = originalColor;
                    Debug.LogWarning($"Hex Code 변환 실패. 타입 이름 '{data.Type}'에 매핑된 색상 적용.");
                }
                else
                {
                    // 3. 모든 시도 실패 시 최종 기본 색상 적용
                    originalColor = Color.white; // 최종 기본값 (흰색)
                    spriteRenderer.color = originalColor;
                    Debug.LogWarning($"색상 '{data.Type}'을 찾을 수 없습니다. 최종 기본 색상 (White) 적용.");
                }
            }


            // set debug position
            debugPosition = startPosition;
        }

        private bool TryGetDefinedColor(string typeString, out Color resultColor)
        {
            // C# 7.0 이상의 switch 표현식 또는 Dictionary를 사용하면 더 깔끔합니다.
            
            // 대소문자 구분 없이 처리 (OrdinalIgnoreCase)
            switch (typeString.ToUpper()) 
            {
                case "STONE":
                    resultColor = Color.gray;
                    return true;
                case "COPPER":
                    // 황동색 (Copper) 예시 Hex 코드를 Color로 변환
                    if (ColorUtility.TryParseHtmlString("#B87333", out resultColor)) 
                    {
                        return true;
                    }
                    break;
                case "IRON":
                    // 철 색상 예시
                    resultColor = new Color(0.7f, 0.7f, 0.7f); 
                    return true;
                
                // 여기에 다른 타입과 색상 매핑을 추가할 수 있습니다.
            }
            
            resultColor = Color.white; // 매핑된 색상이 없는 경우
            return false;
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        // 엔진 컴포넌트 관련 기능 실행
        public void RunEngineComponent()
        {
            // 임시 그래픽 액션 실행
            TempGraphicAction();
        }

        public void DeleteEngineComponent()
        {
            // 엔진 컴포넌트 삭제 처리
            Destroy(this.gameObject);
            // onDestroy에서 presenter.Dispose() 호출
        }

        public void ChosenGraphic()
        {
            spriteRenderer.material.color = Color.green;
        }

        public void DefaultGraphic()
        {
            spriteRenderer.material.color = originalColor;
        }

        public void WireGraphic(int index)
        {
            wireComponent.ActivateWire(index);
        }

        #endregion

            #region private methods
        private void TempGraphicAction()
        {
            // 임시 그래픽 액션 : 색깔을  잠깐 바꿨다가 원래대로
            spriteRenderer.material.DOColor(flashColor, flashDuration) 
                // 2. 변경이 완료된 후 실행될 콜백 지정
            .OnComplete(() =>
            {
                // 콜백에서 원래 색상으로 복귀
                spriteRenderer.material.DOColor(originalColor, flashDuration);
            });

        }

        private Vector2 GetPivot(Vector2Int main, Vector2Int length)
        {
            float pivotX, pivotY;
            pivotX = (2 * main.x + 1f) / (2 * length.x);
            pivotY = (2 * main.y + 1f) / (2 * length.y);

            return new Vector2(pivotX, pivotY);
        }

        
        #endregion

        #region Unity event methods
        private void Awake()
        {
            // init ����� ����
            
        }

        private void Start()
        {
            if(presenter == null)
            {
                Debug.LogWarning("씬에서 직접 EngineComponent를 생성했습니다. 코어로 간주합니다.");
                InitializeCore(debugPosition);
            }
        }

        private void OnDestroy()
        {
            presenter.Dispose();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button != PointerEventData.InputButton.Left)
            {
                return; 
            }

            if(GameManager.Instance.isInBatchMode())
            {
                Debug.Log("Batch Mode에서는 엔진 상세정보를 볼 수 없습니다.");
                return;
            }

            OnClickEngineDetail?.Invoke();
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
