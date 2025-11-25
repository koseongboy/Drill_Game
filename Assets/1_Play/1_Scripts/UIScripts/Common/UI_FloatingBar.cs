using System;
using System.Collections;
using DG.Tweening;
using DrillGame.Core.Ground;
using DrillGame.Core.Managers;
using DrillGame.UI;
using DrillGame.UI.Interface;
using DrillGame.View.Ground;
using UnityEngine;
using TMPro;

namespace DrillGame
{
    public class UI_FloatingBar : MonoBehaviour, UI_IAddressable
    {
        #region Fields & Properties

        [SerializeField]
        private string addressableName;
        
        [SerializeField]
        private TextMeshProUGUI lvlText;
        
        [SerializeField]
        private TextMeshProUGUI depthTxt;
        
        [SerializeField]
        private TextMeshProUGUI researchTxt;
        
        [SerializeField]
        private TextMeshProUGUI playTimeTxt;
        private float totalPlayTime;
        
        [SerializeField]
        private TextMeshProUGUI inputCountTxt;
        
        [SerializeField]
        private TextMeshProUGUI tickCountTxt;

        [SerializeField] 
        private GameObject tickAlert;
        [SerializeField]
        private ParticleSystem tickParticles;
        
        private Tween alertTween;
        
        #endregion

        #region Singleton & initialization
        #endregion
        
        #region getters & setters
        #endregion

        #region public methods
        public void CloseUI()
        {
            return;
            // 얘는 안 꺼요.
        }
        
        public void LinkAddressable(string address)
        {
            // Debug.Log($"{gameObject.name}: addressable 주소 설정 : {address}");
            addressableName = address;
        }
        #endregion

        #region private methods
        #region Observing

        private void OnDepthChanged(int depth)
        {
            depthTxt.text = depth.ToString();
        }
        
        private void OnInputCountChanged(int count)
        {
            inputCountTxt.text = count.ToString();
        }

        private void OnTickCountChanged(int count)
        {
            tickCountTxt.text = count.ToString();
            PlayCoreActiveEffect();
        }
        
        private void OnResearchProgressRateChanged(int researchId, float progress, float progressRate)
        {
            researchTxt.text = (progressRate * 100).ToString("F1") + "%";
        }
        
        /// <summary>
        /// 코어 작동하면, 우측에 빨간 알림에서 파티클이 퍼벙-
        /// </summary>
        public void PlayCoreActiveEffect()
        {
            if (alertTween != null && alertTween.IsActive())
            {
                alertTween.Kill(true);
            }
            
            tickParticles.Play();
            
            RectTransform rt = tickAlert.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            
            float targetScale = 1.8f;
            
            alertTween = rt.DOScale(targetScale, 0.1f)
                .SetEase(Ease.OutQuad)
                .SetLoops(1, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    if (tickParticles != null && tickParticles.isPlaying)
                    {
                        tickParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    }
                    
                    rt.localScale = Vector3.one;
                });
        }

        #endregion
        
        private void UpdateTime()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalPlayTime);
            int totalDays = (int)timeSpan.TotalDays;
            string timeString = string.Format(
                "{0:00}:{1:00}:{2:00}:{3:00}",
                totalDays,                           // DD (일)
                timeSpan.Hours,                      // HH (시)
                timeSpan.Minutes,                    // MM (분)
                timeSpan.Seconds                     // SS (초)
            );
            
            playTimeTxt.text = timeString;
        }
        
        private IEnumerator UpdatePlayTimeTxt()
        {
            while (true)
            {
                totalPlayTime = Time.time;
                UpdateTime();
                yield return new WaitForSeconds(1f); // 1초 대기
            }
        }
        #endregion

        #region Unity event methods

        private void OnEnable()
        {
            GroundComponent.Instance.OnDepthChanged += OnDepthChanged;
            InputCountManager.Instance.OnInputCountChanged += OnInputCountChanged;
            InputCountManager.Instance.OnTickCountChanged += OnTickCountChanged;
            ResearchManager.Instance.OnResearchProgressChanged += OnResearchProgressRateChanged; 
        }

        private void OnDisable()
        {
            // 여기서 NullReferenceException 발생할텐데, 무시해도 됨. (아마도)
            GroundComponent.Instance.OnDepthChanged -= OnDepthChanged;
            InputCountManager.Instance.OnInputCountChanged -= OnInputCountChanged;
            InputCountManager.Instance.OnTickCountChanged -= OnTickCountChanged;
            ResearchManager.Instance.OnResearchProgressChanged -= OnResearchProgressRateChanged;
        }
        
        private void Start()
        {
            // PlayTime 타이머
            StartCoroutine(UpdatePlayTimeTxt());
        }
        #endregion


    }
}
