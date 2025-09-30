using UnityEngine;
using DrillGame.Data;
using System;

namespace DrillGame.Components
{
    public class CSV_Loader_forDEV : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        async void Start()
        {
            Facility_Data facilityData;
            facilityData = await Facility_Data.CreateAsync();
            Tuple<int, int> size = facilityData.GetSize(1);
            Debug.Log("Size for facility id 1: " + size);
            Tuple<int, int> size2 = facilityData.GetSize(2);
            Debug.Log("Size for facility id 1: " + size2);
        
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
