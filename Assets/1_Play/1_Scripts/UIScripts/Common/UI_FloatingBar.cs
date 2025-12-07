using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DrillGame.Core.Ground;
using DrillGame.Core.Managers;
using DrillGame.Managers;
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

        private void OnLevelChanged(int level)
        {
            lvlText.text = level.ToString();
        }

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
                totalPlayTime += 1;
                UpdateTime();
                yield return new WaitForSeconds(1f); // 1초 대기
            }
        }

        private void SavePlayTime()
        {
            SaveManager.Instance.SaveDailyPlayTimeData((int)totalPlayTime);
        }
        
        private void LoadPlayTime()
        {
            var savedTodayPlayTime = SaveManager.Instance.LoadTodayPlayTime();
            totalPlayTime = savedTodayPlayTime;
        }
        #endregion

        #region Unity event methods

        private void OnEnable()
        {
            OnLevelChanged( CoreManager.Instance.GetCoreLevel() );
            GroundComponent.Instance.OnDepthChanged += OnDepthChanged;
            InputCountManager.Instance.OnInputCountChanged += OnInputCountChanged;
            InputCountManager.Instance.OnTickCountChanged += OnTickCountChanged;
            ResearchManager.Instance.OnResearchProgressChanged += OnResearchProgressRateChanged; 
            CoreManager.Instance.OnCoreLevelChanged += OnLevelChanged;
            SaveManager.OnRequestAllDataSave += SavePlayTime;
        }

        private void OnDisable()
        {
            return;
            // 여기서 NullReferenceException 발생할텐데, 무시해도 됨. (아마도)
            GroundComponent.Instance.OnDepthChanged -= OnDepthChanged;
            InputCountManager.Instance.OnInputCountChanged -= OnInputCountChanged;
            InputCountManager.Instance.OnTickCountChanged -= OnTickCountChanged;
            ResearchManager.Instance.OnResearchProgressChanged -= OnResearchProgressRateChanged;
            CoreManager.Instance.OnCoreLevelChanged -= OnLevelChanged;
            SaveManager.OnRequestAllDataSave -= SavePlayTime;
        }
        
        private void Start()
        {
            LoadPlayTime();
            
            // PlayTime 타이머
            StartCoroutine(UpdatePlayTimeTxt());
        }

        #endregion

        #region DEV

        [ContextMenu("SetInputCount 5")]
        public void SetInputCount_5()
        {
            InputCountManager.Instance.SetInputCount();
        }

        #endregion

    }
}
