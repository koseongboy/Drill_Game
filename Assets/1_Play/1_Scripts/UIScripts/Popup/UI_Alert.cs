using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DrillGame.UI;
using DrillGame.UI.Interface;
using TMPro;
using UnityEngine;

namespace DrillGame
{
    public class UI_Alert : MonoBehaviour, UI_IAddressable
    {
        #region Fields & Properties

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI ui_text;
        
        private Vector2 startPosition;

        [SerializeField] private float moveTime = 0.1f;
        [SerializeField] private float stayTime = 3f;
        [SerializeField] private float moveDistance = 100f;
        
        private Coroutine autoHideCoroutine;
        
        #endregion

        #region Singleton & initialization
        public static UI_Alert Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
            
            Init();
        }

        private void Init()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            
            startPosition = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0;
            rectTransform.anchoredPosition = startPosition - new Vector2(0, moveDistance);
        }
        
        [SerializeField]
        private string addressableName;
        public void LinkAddressable(string address)
        {
            addressableName = address;
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void UpdateUI(string text)
        {
            ui_text.text = text;
        }
        #endregion

        #region private methods

        
        private void OpenAction()
        {
            DOTween.Kill(this.transform);
            gameObject.SetActive(true);
            
            // 1. 시퀀스 생성
            Sequence sequence = DOTween.Sequence();
            sequence.SetTarget(this.transform); 

            rectTransform.anchoredPosition = startPosition - new Vector2(0, moveDistance);
            canvasGroup.alpha = 0;

            sequence.Append(canvasGroup.DOFade(1, moveTime));
            sequence.Join(rectTransform.DOAnchorPos(startPosition, moveTime).SetEase(Ease.OutQuad));

            // 1초 동안 대기
            sequence.AppendInterval(stayTime);

            sequence.Append(canvasGroup.DOFade(0, moveTime));
            sequence.Join(rectTransform.DOAnchorPos(startPosition + new Vector2(0, moveDistance), moveTime).SetEase(Ease.InQuad));

            sequence.OnComplete(() =>
            {
                rectTransform.anchoredPosition = startPosition; 
                canvasGroup.alpha = 0;
                ui_text.text = "";
                gameObject.SetActive(false); 
            });
            sequence.Play();
        }
        private IEnumerator AutoHideTimer()
        {
            yield return new WaitForSeconds(stayTime);
        
            // 3초가 지나면 자동 호출
            HideAlert();
        }

        // ★ 사용자 클릭 시 호출되는 함수
        public void OnAlertClicked()
        {
            // 클릭되었으므로 자동 사라짐 타이머 중단
            if (autoHideCoroutine != null) StopCoroutine(autoHideCoroutine);
        
            HideAlert();
        }

        // 알림창 사라지는 애니메이션 (클릭 또는 타이머 종료 시 호출)
        private void HideAlert()
        {
            // 1. 사라지기 (Fade Out & Move Up) Sequence
            Sequence disappearSequence = DOTween.Sequence();
            disappearSequence.SetTarget(this.transform);

            // 투명도: 1 -> 0
            disappearSequence.Join(canvasGroup.DOFade(0, moveTime));
            // 위치: startPosition -> 위로 moveDistance만큼 이동
            disappearSequence.Join(rectTransform.DOAnchorPos(startPosition + new Vector2(0, moveDistance), moveTime).SetEase(Ease.InQuad));

            // 2. 완료 시 처리
            disappearSequence.OnComplete(() =>
            {
                // Raycast 차단 해제 및 오브젝트 비활성화로 상태 초기화
                canvasGroup.blocksRaycasts = false; 
                rectTransform.anchoredPosition = startPosition; 
                canvasGroup.alpha = 0;
                gameObject.SetActive(false); 
            });

            disappearSequence.Play();
        }
        #endregion

        #region Unity event methods

        private void OnEnable()
        {
            OpenAction();
        }
        #endregion

    }
}
