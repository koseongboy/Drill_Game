using System;
using DG.Tweening;
using DrillGame.UI;
using DrillGame.UI.Interface;
using UnityEngine;
using TMPro;

namespace DrillGame
{
    public class UI_FloatingBar : MonoBehaviour, UI_IAddressable, IInputCountObserver
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
        private TextMeshProUGUI inputCountTxt;
        
        [SerializeField]
        private TextMeshProUGUI tickCountTxt;

        [SerializeField] 
        private GameObject tickAlert;
        [SerializeField]
        private ParticleSystem tickParticles;
        
        private Tween alertTween;
        
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

        public void UpdateTest() {
            Debug.Log("Updated UI 테스트 호출");
            UpdateUITxt();
        }

        public void LinkAddressable(string address)
        {
            // Debug.Log($"{gameObject.name}: addressable 주소 설정 : {address}");
            addressableName = address;
        }
        
        // InputCount 옵저빙
        public void OnInputCountChanged(int count)
        {
            tickCountTxt.text = count.ToString();
        }
        #endregion

        #region private methods
        
        private void UpdateUITxt() {
            lvlText.text = "77"; //TODO
            depthTxt.text = "777"; //TODO
            researchTxt.text = "77%"; //TODO
            inputCountTxt.text = "77777777"; //TODO
            tickCountTxt.text = "7777777"; //TODO
        }


        #endregion

        #region Unity event methods

        private void Start()
        {
            InputCountManager.Instance.AddInputCountObserver(this);
        }

        #endregion
    }
}
