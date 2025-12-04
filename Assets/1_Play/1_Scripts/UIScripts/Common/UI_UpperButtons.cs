using System.Collections;
using DrillGame.Core.Managers;
using DrillGame.Managers;
using DrillGame.UI;
using DrillGame.UI.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_UpperButtons : MonoBehaviour, UI_IAddressable
    {
        [SerializeField]
        private string addressableName;
        
        [SerializeField] private UI_Inventory ui_Inventory;

        [SerializeField] private GameObject editingHighlight;
        
        [SerializeField] private Image viewStateIcon;
        [SerializeField] private TextMeshProUGUI viewStateTxt;

        private bool isEditing = false;
        private bool isInventoryOpen = false;
        private bool isTryQuit = false;
        
        public void LinkAddressable(string address)
        {
            addressableName = address;
        }

        public void TryQuitGame()
        {
            if (isTryQuit)
            {
                Debug.Log("Quit Game");
                Application.Quit();
            }
            
            isTryQuit = true;
            UILoader.Instance.ShowAlert("게임을 종료하려면\n다시 한 번 버튼을 누르세요.");
            StartCoroutine(StopQuit());
        }

        private IEnumerator StopQuit()
        {
            yield return new WaitForSeconds(3f);
            isTryQuit = false;
        }
        
        

        public void InventoryButtonPressed()
        {
            if (isInventoryOpen)
            {
                UI_ResourceInventory.Instance.CloseUI();
            }
            else
            {
                UILoader.Instance.ShowUI("UI_ResourceInventory");
            }
            isInventoryOpen = !isInventoryOpen;
        }

        public void EditButtonPressed()
        {
            isEditing = !isEditing;
            editingHighlight.SetActive(isEditing);
            if (isEditing)
            {
                // 전체보기일 경우, 상부로 이동
                if (GameViewManager.Instance.GetViewState() == GameViewManager.ViewState.All)
                {
                    UpdateUI_ViewStateButton(GameViewManager.ViewState.FacilityOnly);
                    GameViewManager.Instance.SetViewState(GameViewManager.ViewState.FacilityOnly);
                }
                
                // 인벤토리 띄우기
                UILoader.Instance.ShowUI("UI_Inventory");
            }
            else
            {
                // 인벤토리 내리기
                UILoader.Instance.HideUI("UI_Inventory");
            }
        }

        public void ViewStateButtonPressed()
        {
            var currentState = GameViewManager.Instance.GetViewState();
            GameViewManager.ViewState next;
            
            if (isEditing) // 배치 모드 -> 상부 / 하부 2개로만 토글
            {
                return;
            }else // 일반 모드 -> 전체 / 상부 / 하부 3개로 토글
            {
                next = currentState switch
                {
                    GameViewManager.ViewState.All => GameViewManager.ViewState.FacilityOnly,
                    GameViewManager.ViewState.FacilityOnly => GameViewManager.ViewState.EngineOnly,
                    GameViewManager.ViewState.EngineOnly => GameViewManager.ViewState.All,
                    _ => GameViewManager.ViewState.All
                };
            }

            GameViewManager.Instance.SetViewState(next);
            UpdateUI_ViewStateButton(next);
        }

        private void Init()
        {
            isEditing = false;
            editingHighlight.SetActive(false);
            UpdateUI_ViewStateButton( GameViewManager.Instance.GetViewState() );
        }

        private void UpdateUI_ViewStateButton(GameViewManager.ViewState next)
        {
            UpdateIcon_ViewStateButton(next);
            UpdateTxt_ViewStateButton(next);
        }

        private void UpdateIcon_ViewStateButton(GameViewManager.ViewState next)
        {
            viewStateIcon.sprite = next switch
            {
                GameViewManager.ViewState.All => Resources.Load<Sprite>("UISprite/Button/AllView"),
                GameViewManager.ViewState.FacilityOnly => Resources.Load<Sprite>("UISprite/Button/UpperView_Facility"),
                GameViewManager.ViewState.EngineOnly => Resources.Load<Sprite>("UISprite/Button/LowerView_Engine")
            };
        }

        private void UpdateTxt_ViewStateButton( GameViewManager.ViewState viewState )
        {
            viewStateTxt.text = viewState switch
            {
                GameViewManager.ViewState.All => "전체",
                GameViewManager.ViewState.FacilityOnly => "상부",
                GameViewManager.ViewState.EngineOnly => "하부"
            };
        }
    }
}
