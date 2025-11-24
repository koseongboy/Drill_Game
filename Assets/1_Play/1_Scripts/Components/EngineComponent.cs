using DG.Tweening;
using DrillGame.Core.Engine;
using DrillGame.Core.Presenter;
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
        private Vector2Int debugPosition;   // -> �̰� ����� ���Ŀ��� ���������Ұ� ���� �ʳ�? �����̼��� static �� data�ϱ�
        [SerializeField]
        private List<Vector2Int> debugFormation = new();
        [SerializeField]
        private string engineType = "BasicEngine";

        private EnginePresenter presenter;
        public Action OnClickEngineDetail { get; set; }


        // for temp graphic action
        private SpriteRenderer spriteRenderer;
        private Color originalColor;
        private Color flashColor = Color.yellow;
        private float flashDuration = 0.15f;

        private Color onMouseColor = Color.cyan;

        #endregion

        #region Singleton & initialization
        public void Initialize(Vector2Int startPosition, int itemId=0, int unitId=0)
        {
            // for Test ���� ���丮 �������� �и� �ʿ� -> �ٵ� ���� ���� �ൿ ������ ����..
            if (engineType != "BasicEngine")
            {
                Debug.LogWarning("����� BasicEngine�� �����մϴ�. �⺻������ �����մϴ�.");
                engineType = "BasicEngine";
            }

            EngineEntity engineEntity = new EngineEntity(startPosition, debugFormation, itemId, unitId);

            presenter = new EnginePresenter(this, engineEntity);

            OnClickEngineDetail = () => {
                presenter.RequestEngineDetail();
                // Ȯ�强�� ���� ���ٽ� ���
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
        // ���� ������Ʈ ���� ��� ����
        public void RunEngineComponent()
        {
            // �ӽ� �׷��� �׼� ����
            TempGraphicAction();
        }

        public void DeleteEngineComponent()
        {
            // ���� ������Ʈ ���� ó��
            Destroy(this.gameObject);
            // onDestroy���� presenter.Dispose() ȣ��
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
        private void TempGraphicAction()
        {
            // �ӽ� �׷��� �׼� : ������  ��� �ٲ�ٰ� �������
            spriteRenderer.material.DOColor(flashColor, flashDuration)
            // 2. ������ �Ϸ�� �� ����� �ݹ� ����
            .OnComplete(() =>
            {
                // �ݹ鿡�� ���� �������� ����
                spriteRenderer.material.DOColor(originalColor, flashDuration);
            });



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
                Debug.LogWarning("������ ���� EngineComponent�� �����߽��ϴ�. �׽�Ʈ�� �⺻ ������ �����մϴ�.");
                Initialize(debugPosition);
            }
        }

        private void OnDestroy()
        {
            presenter.Dispose();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // ���� �׽�Ʈ �뵵�� ������ ����ٰ� �ɾ�ξ��µ� �׷��� ��ġ���ڸ��� ������ ��� Ŭ������ �ٲ���
            if(eventData.button != PointerEventData.InputButton.Middle)
            {
                return; 
            }
            OnClickEngineDetail?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = onMouseColor;
        }

        // IPointerExitHandler�� �ʼ� �޼��� ����
        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = originalColor;
        }



        #endregion
    }
}
