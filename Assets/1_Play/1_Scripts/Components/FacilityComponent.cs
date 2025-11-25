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
using System.Security.Cryptography;
using DrillGame.View.Ground;
using DrillGame.Core.Ground;

namespace DrillGame.View.Facility
{
    public class FacilityComponent : MonoBehaviour, IPointerClickHandler, IDrillGameObjectInit, IDrillGameDefaultGrapic, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields & Properties

        //���� ����

        [SerializeField]
        public int debugId = 101011;
        [SerializeField]
        private Vector2Int debugPosition; // -> �̰� ����� ���Ŀ��� ���������Ұ� ���� �ʳ�? �����̼��� static �� data�ϱ�

        
        List<Vector2Int> formation = new();
        public string EntityClassName = "FacilityEntity";
        public string Name;
        public string DisplayName;
        public string Type;
        public int Level;
        public string BuildResourceId;
        public string BuildResourceCount;
        public string InputItemId;
        public string InputItemCount;
        public int OutputItemId;
        public int OutputItemCount;
        

        private FacilityPresenter presenter;

        public Action OnClickFacilityDetail { get; set; }

        // graphic action ���� �ӽ� �ʵ�
        private SpriteRenderer spriteRenderer;
        private Color originalColor;

        private Color onMouseColor = Color.cyan;

        #endregion

        #region Singleton & initialization
        public void Initialize(Vector2Int startPosition)
        {
            Facility_Data_ data = ScriptableObjectManager.Instance.GetData<Facility_Data_>(debugId);
            formation = data.GetCoordinates();
            EntityClassName = data.EntityClassName;
            Name = data.Name;
            DisplayName = data.DisplayName;
            Type = data.Type;
            Level = data.Level;
            BuildResourceId = data.BuildResourceId;
            BuildResourceCount = data.BuildResourceCount;
            InputItemId = data.InputItemId;
            InputItemCount = data.InputItemCount;
            OutputItemId = data.OutputItemId;
            OutputItemCount = data.OutputItemCount;

            

            // ��Ʈ������ ���� Ŭ���� ������ ���� facility action �ν��Ͻ� ����
            string fullEntityClassName = "DrillGame.Core.Facility." + EntityClassName;
            Type type = System.Type.GetType(fullEntityClassName);
            Debug.Log("Facility Action Type : " + type);
            if (type == null)
            {
                Debug.LogError($"Facility action class '{fullEntityClassName}' not found. Using default action.");
                type = typeof(FacilityEntity); // �⺻ �׼����� ��ü
            }
            object[] parameters = new object[] { startPosition, formation, Level };
            FacilityEntity facilityEntity = Activator.CreateInstance(type, parameters) as FacilityEntity;
            presenter = new FacilityPresenter(this, facilityEntity);

            OnClickFacilityDetail = () => {
                presenter.RequestFacilityDetail();
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
        public void RunFacilityComponent(int intensity)
        {
            // �ӽ� �׷��� �׼� ����
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
            // �ӽ� �׷��� �׼�
            transform.DOKill(true);
            transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0) * intensity, 0.1f, 10, 1);
        }
        #endregion

        #region Unity event methods
        private void Awake()
        {
            // init ����� ����

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
            if (eventData.button != PointerEventData.InputButton.Middle)
            {
                return;
            }

            OnClickFacilityDetail?.Invoke();

            Debug.Log("FacilityComponent clicked : UI �ʿ��ؿ�");
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
