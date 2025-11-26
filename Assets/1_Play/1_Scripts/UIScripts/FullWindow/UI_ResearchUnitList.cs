using System.Collections.Generic;
using UnityEngine;

namespace DrillGame._1_Play._1_Scripts.UIScripts.FullWindow
{
    public class UI_ResearchUnitList : MonoBehaviour
    {
        [SerializeField]
        private int techID;
        [SerializeField]
        private List<UI_ResearchUnit> unitList = new List<UI_ResearchUnit>();

        [SerializeField] private string name;

        private void OnEnable()
        {
            foreach (var unit in unitList)
            {
                unit.SetData(techID, name);
            }
        }
    }
}