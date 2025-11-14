using System;
using System.Collections;
using DG.Tweening;
using DrillGame.UI;
using DrillGame.UI.Interface;
using UnityEngine;
using TMPro;

namespace DrillGame
{
    public class UI_FloatingBar : MonoBehaviour, UI_IAddressable, IInputCountObserver, IResearchObserver
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
        public static UI_FloatingBar Instance { get; private set; }
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
        }
        #endregion
        
        #region getters & setters
        #endregion

        #region public methods
        public void CloseUI()
        {
            return;
            // 얘는 안 꺼요.

            // Debug.Log($"{gameObject.name}: UI 종료 시도, addressable 주소 : {addressableName}");
            // UILoader.Instance.HideUI(addressableName);
        }
        
        /// <summary>
        /// 코어 작동하면, 우측에 빨간 알림에서 파티클이 퍼벙-
        /// </summary>
        public void AlertCoreActive()
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

        public void LinkAddressable(string address)
        {
            // Debug.Log($"{gameObject.name}: addressable 주소 설정 : {address}");
            addressableName = address;
        }
        
        #region Observing
        public void OnInputCountChanged(int count)
        {
            inputCountTxt.text = count.ToString();
        }

        public void OnTickCountChanged(int count)
        {
            tickCountTxt.text = count.ToString();
        }
        public void OnResearchProgressChanged(float progress)
        {
            researchTxt.text = progress.ToString("F1") + "%";
        }
        #endregion
        #endregion

        #region private methods
        #endregion

        #region Unity event methods

        private void Start()
        {
            InputCountManager.Instance.AddInputCountObserver(this);
            
            // PlayTime 타이머
            StartCoroutine(UpdatePlayTimeTxt());
            
            var researchProgress = ResearchManager.Instance.AddResearchObserver(this);
            OnResearchProgressChanged(researchProgress);
        }
        #endregion


    }
}
