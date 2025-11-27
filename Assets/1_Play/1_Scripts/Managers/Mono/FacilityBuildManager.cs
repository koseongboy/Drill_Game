using System;
using System.Collections.Generic;
using DrillGame.Core.Managers;
using UnityEngine;

namespace DrillGame._1_Play._1_Scripts.Managers.Mono
{
    public class FacilityBuildManager : MonoBehaviour
    {
        #region Fields & Properties
        [SerializeField]
        private List<int> allFacilityItemIds = new List<int>();
        
        private Dictionary<Facility_Data_.FacilityType, List<int>> unlockedFacilityItemIds = new Dictionary<Facility_Data_.FacilityType, List<int>>();
        #endregion

        #region Singleton & initialization
        public static FacilityBuildManager Instance { get; private set; }
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
            
            allFacilityItemIds = LoadFacilityItemIds();
        }
        #endregion

        #region getters & setters

        public Dictionary<Facility_Data_.FacilityType, List<int>> GetUnlockedFacilityItemIds()
        {
            UpdateUnlockedFacility();
            return unlockedFacilityItemIds;
        }
        #endregion

        #region public methods

        [ContextMenu("해금된 시설 업데이트")]
        public void UpdateUnlockedFacility()
        {
            unlockedFacilityItemIds.Clear(); 
            unlockedFacilityItemIds = LoadUnlockedFacilityItemIds( allFacilityItemIds );
        }
        #endregion

        #region private methods

        private List<int> LoadFacilityItemIds()
        {
            var allItemData = ScriptableObjectManager.Instance.GetAllData<Item_Data_>();
            List<int> list = new List<int>();
            foreach (var kvp in allItemData)
            {
                var itemData = (Item_Data_)kvp.Value;
                if (itemData.GetItemType_Enum() == InventoryManager.ItemType.Facility)
                {
                    list.Add(kvp.Key);
                }
            }

            return list;
        }

        private Dictionary<Facility_Data_.FacilityType, List<int>> LoadUnlockedFacilityItemIds( List<int> facilityItemIds )
        {
            Dictionary<Facility_Data_.FacilityType, List<int>> dict = new Dictionary<Facility_Data_.FacilityType, List<int>>();
            dict.Add(Facility_Data_.FacilityType.Miner, new List<int>());
            dict.Add(Facility_Data_.FacilityType.Processor, new List<int>());
            dict.Add(Facility_Data_.FacilityType.Laboratory, new List<int>());
            dict.Add(Facility_Data_.FacilityType.ResourceMerger, new List<int>());
            dict.Add(Facility_Data_.FacilityType.EngineMerger, new List<int>());
            
            foreach (var itemId in facilityItemIds)
            {
                var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>( itemId );
                var facilityData = ScriptableObjectManager.Instance.GetData<Facility_Data_>( itemData.EntityId );

                if (facilityData.RequireResearchId != 0 && !ResearchManager.Instance.IsResearchDone(facilityData.RequireResearchId))
                {
                    continue;
                }
                if( facilityData.RequireCoreLevel != 0 ) // TODO : 코어 레벨 체크하는 게 있어야.
                {
                    continue;
                }

                if (facilityData.GetFacilityType_Enum() == Facility_Data_.FacilityType.Drill)
                {
                    continue;
                }
                
                dict[facilityData.GetFacilityType_Enum()].Add(itemId);
            }

            return dict;
        }
        #endregion

        #region Unity event methods

        #endregion
    }
}