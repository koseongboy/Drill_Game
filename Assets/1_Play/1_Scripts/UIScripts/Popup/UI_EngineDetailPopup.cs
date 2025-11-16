using System.Collections.Generic;
using DG.Tweening;
using DrillGame.Core.Engine;
using DrillGame.Data;
using DrillGame.Managers;
using DrillGame.UI;
using UnityEngine;
using UnityEngine.UIElements;

using DrillGame.UI.Interface;
using TMPro;

namespace DrillGame
{
    public class UI_EngineDetailPopup : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField]
        private string addressableName;
        
        [SerializeField]
        private TextMeshProUGUI titleTxt;
        [SerializeField]
        private TextMeshProUGUI descTxt;
        [SerializeField]
        private Image iconImg;
        

        private EngineEntity engineEntity;
        #endregion

        #region getters & setters

        public void SetEngineEntity(EngineEntity entity)
        {
            engineEntity = entity;
            UpdateDetail(engineEntity);
        }
        #endregion

        #region public methods
        public void MoveEngineOnBoard()
        {
            Debug.Log("MoveEngineOnBoard 진입.");
            // 영욱 여기야

            engineEntity.MoveEntity();
        }
        
        public void DeleteEngineOnBoard()
        {
            // TODO : 진짜로 철거할 거냐고 물어보기 (Confirm UI)
            // 굳이 필요할까? 철거의 리스크가 전혀 없는 구조 아닌가? 그리고 이동도 저거 철거 후 인벤에서 다시 꺼내오기랑 같은 구조임 ㅋㅋ
            Debug.Log("DeleteEngineOnBoard 진입.");

            // 영욱 여기야
            // 확인

            engineEntity.DeleteEntity();
        }

        public void CloseUI()
        {
            // Debug.Log($"{gameObject.name}: UI 종료 시도, addressable 주소 : {addressableName}");
            CloseAction();
            // UiLoader.HideUI()는 위의 CloseAction내에서, 애니메이션 다 끝나면 호출함.
        }
        
        public void LinkAddressable(string address)
        {
            Debug.Log($"{gameObject.name}: addressable 주소 설정 : {address}");
            addressableName = address;
        }
        #endregion

        #region private methods
        private void UpdateDetail(EngineEntity entity)
        {
            // var id = entity.GetEngineId();
            var id = "201003";  // Test
            var data = DataLoadManager.Instance.EngineData.GetDataById(id);

            titleTxt.text = data["DisplayName"];
            descTxt.text = data["Desc"];
            // TODO : 파일명으로 이미지 불러오는 로직
        }
        
        private void OpenAction()
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

        private void CloseAction() {
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

        private void OnEnable()
        {
            OpenAction();
        }
        #endregion

    }
}
