using System;
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

        
        public void OpenAction()
        {
            // 1. 초기 상태 리셋: 애니메이션 중단 및 초기 위치로 스냅 (Fresh Start 보장)
            DOTween.Kill(rectTransform);
            if (autoHideCoroutine != null)
            {
                StopCoroutine(autoHideCoroutine);
                autoHideCoroutine = null;
            }
            
            // 주의: blocksRaycasts는 애니메이션이 끝난 후 켜야 합니다.
            canvasGroup.blocksRaycasts = false; 
            gameObject.SetActive(true);
            
            // 리셋 위치
            canvasGroup.alpha = 0;
            rectTransform.anchoredPosition = startPosition - new Vector2(0, moveDistance);
            Debug.Log("UI 리셋 위치: " + rectTransform.anchoredPosition);
            
            // 2. 나타나기 Sequence 생성
            Sequence appearSequence = DOTween.Sequence();
            appearSequence.SetTarget(rectTransform); 

            appearSequence.Append(canvasGroup.DOFade(1, moveTime));
            appearSequence.Join(rectTransform.DOAnchorPos(startPosition, moveTime).SetEase(Ease.OutQuad));

            // 3. 나타나기 완료 시 처리: 상호작용 활성화 및 타이머 시작
            appearSequence.OnComplete(() =>
            {
                // ★ 상호작용 활성화: 이제 클릭을 감지합니다.
                canvasGroup.blocksRaycasts = true; 
                
                // ★ 자동 사라짐 타이머 시작 (3초 대기)
                autoHideCoroutine = StartCoroutine(AutoHideTimer());
            });
            appearSequence.Play();
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
            autoHideCoroutine = null; // 코루틴 참조 해제
        
            HideAlert();
        }

        // 알림창 사라지는 애니메이션 (클릭 또는 타이머 종료 시 호출)
        private void HideAlert()
        {
            DOTween.Kill(rectTransform); // 중복 호출 방지
            
            // 상호작용 비활성화: 사라지는 동안 클릭을 막습니다.
            canvasGroup.blocksRaycasts = false; 

            // 1. 사라지기 Sequence
            Sequence disappearSequence = DOTween.Sequence();
            disappearSequence.SetTarget(rectTransform);

            disappearSequence.Join(canvasGroup.DOFade(0, moveTime));
            disappearSequence.Join(rectTransform.DOAnchorPos(startPosition + new Vector2(0, moveDistance), moveTime).SetEase(Ease.InQuad));

            // 2. 완료 시 처리
            disappearSequence.OnComplete(() =>
            {
                // 최종 상태 초기화 및 비활성화
                rectTransform.anchoredPosition = startPosition; 
                canvasGroup.alpha = 0;
                ui_text.text = "";
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
