using System;
using System.Collections;
using System.Collections.Generic;
using DrillGame.UI;
using DrillGame.UI.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_ResourceConverter : MonoBehaviour, UI_IAddressable
    {
        #region Singleton & initialization
                
        private string addressableName;
        public void LinkAddressable(string address)
        {
            addressableName = address; 
        }
        #endregion
        
        #region Fields & Properties
        [SerializeField] private RectTransform contentRectTransform;
        [SerializeField] private GameObject ui_piece;
        [SerializeField] private RectTransform ui_pieceParent;
        
        private List<int> researchIds = new List<int>
        {
            30001,
            30006,
            30011,
            30016,
            30021,
            30026
        };
        private List<int> unlockedOutputResourceIds = new List<int>();
        
        private List<GameObject> resourcePieces = new List<GameObject>();
        #endregion
        
        #region getters & setters
        #endregion

        #region public methods
        public virtual void CloseUI()
        {
            ClearAllPieces();
            CloseAction();
        }
        #endregion

        #region private methods

        private void LoadOutputResourceList()
        {
            for (int i = 0; i < researchIds.Count; i++)
            {
                var researchId = researchIds[i];
                if (!ResearchManager.Instance.IsResearchDone(researchId))
                {
                    break;
                }
                
                unlockedOutputResourceIds.Add(1001 + i);
                unlockedOutputResourceIds.Add(1002 + i);
            }
        }

        private void UpdateUI_ResourceList()
        {
            resourcePieces = new List<GameObject>();
            
            // 새 자원
            foreach (var researchId in unlockedOutputResourceIds)
            {
                var resourcePiece = Instantiate(ui_piece, ui_pieceParent);
                // resourcePiece.GetComponent<UI_EngineMergePiece>().SetData_Level(researchId);
                resourcePieces.Add(resourcePiece); 
            }
            
            StartCoroutine(UpdateLayout());
        }
        
        private IEnumerator UpdateLayout()
        {
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
        }
        
        private void ClearAllPieces()
        {
            foreach (var obj in resourcePieces)
            {
                Destroy(obj);
            }
        }
        
        private void CloseAction()
        {
            UILoader.Instance.HideUI(addressableName);
        }
        #endregion

        #region Unity event methods

        private void OnEnable()
        {
            LoadOutputResourceList();
            UpdateUI_ResourceList();
        }

        #endregion
    }
}