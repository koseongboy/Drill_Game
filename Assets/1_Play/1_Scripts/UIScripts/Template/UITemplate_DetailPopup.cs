using DG.Tweening;
using DrillGame.Core.Engine;
using DrillGame.Managers;
using DrillGame.UI;
using DrillGame.UI.Interface;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace DrillGame
{
    public class UITemplate_DetailPopup : MonoBehaviour, UI_IAddressable
    {
        #region Fields & Properties

        [SerializeField]
        protected string addressableName;
        
        [SerializeField]
        protected TextMeshProUGUI titleTxt;
        [SerializeField]
        protected TextMeshProUGUI descTxt;
        [SerializeField]
        protected Image iconImg;
        [SerializeField]
        protected GameObject onSynergyIcon;

        protected bool isOnSynergy = false;
        #endregion

        #region getters & setters
        public virtual void SetData(object entity)
        {
        }

        public void SetIsOnSynergy(bool isOnSynergy)
        {
            this.isOnSynergy = isOnSynergy;
            onSynergyIcon.SetActive(this.isOnSynergy);
        }
        #endregion

        #region public methods
        public virtual void MoveOnBoard()
        {
        }
        
        public virtual void DeleteOnBoard()
        {
            // TODO : 진짜로 철거할 거냐고 물어보기 (Confirm UI)
            // 명준 : 잘못 눌러서 지워버리면, 다시 깔기 귀찮잖아.
            // Debug.Log("DeleteEngineOnBoard 진입.");
        }
        
        public virtual void LinkAddressable(string address)
        {
            Debug.Log($"{gameObject.name}: addressable 주소 설정 : {address}");
            addressableName = address;
        }
        
        public virtual void CloseUI()
        {
            CloseAction();
            // UiLoader.HideUI()는 위의 CloseAction내에서, 애니메이션 다 끝나면 호출함.
        }
        #endregion

        #region private methods
        protected virtual void UpdateDetail(string name, string desc, string iconName)
        {
            titleTxt.text = name;
            descTxt.text = desc;
            
            iconImg.sprite = SpriteLoader.Instance.LoadSprite(iconName).Result;
        }
        
        protected void OpenAction()
        {
            RectTransform rt = GetComponent<RectTransform>();
            
            Vector2 startPos = rt.anchoredPosition;
            Vector2 targetPos = new Vector2(startPos.x, startPos.y + 100f);
            rt.anchoredPosition = startPos;
            rt.DOAnchorPos(targetPos, 0.1f)
                .SetEase(Ease.OutBack);
            
            rt.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(Vector2.one, 0.1f)
                .SetEase(Ease.OutBack);
        }

        protected void CloseAction() {
            RectTransform rt = GetComponent<RectTransform>();
            
            Vector2 startPos = rt.anchoredPosition;
            Vector2 targetPos = new Vector2(startPos.x, startPos.y - 100f);
            rt.anchoredPosition = startPos;
            rt.DOAnchorPos(targetPos, 0.1f)
                .SetEase(Ease.Linear);
            
            Vector3 targetScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(targetScale, 0.1f)
                .SetEase(Ease.Linear)
                .OnComplete(() => {
                    UILoader.Instance.HideUI(addressableName);
                });
        }
        #endregion

        #region Unity event methods

        protected virtual void OnEnable()
        {
            OpenAction();
        }
        #endregion
    }
}
