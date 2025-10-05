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
            /*
            Engine_Data engineData = await Engine_Data.CreateAsync();
            var coords = engineData.GetCoordinate("normal-3");
            foreach (var coord in coords)
            {
                Debug.Log($"Coordinate: ({coord.Item1}, {coord.Item2})");
            }
            var coords2 = engineData.GetCoordinate("special-2");
            foreach (var coord in coords2)
            {
                Debug.Log($"Coordinate: ({coord.Item1}, {coord.Item2})");
            }


            Facility_Data facilityData = await Facility_Data.CreateAsync();
            var facilityStruct = facilityData.GetFacility_Structure("iron-1");
            Debug.Log($"Facility ID: {facilityStruct.id}, Name: {facilityStruct.name}");
            foreach (var coord in facilityStruct.coordinates)
            {
                Debug.Log($"Coordinate: ({coord.Item1}, {coord.Item2})");
            }
            */
            Ground_Data groundData = await Ground_Data.CreateAsync();
            var groundStruct = groundData.GetGround_Structure(1);
            Debug.Log($"Ground ID: {groundStruct.id}, Start Depth: {groundStruct.start_depth}, End Depth: {groundStruct.end_depth}, HP: {groundStruct.hp}, Color: {groundStruct.color}");
            foreach (var item in groundStruct.drop_items)
            {
                Debug.Log($"Drop Item: {item}");
            }
            groundStruct = groundData.GetGround_Structure(2);
            Debug.Log($"Ground ID: {groundStruct.id}, Start Depth: {groundStruct.start_depth}, End Depth: {groundStruct.end_depth}, HP: {groundStruct.hp}, Color: {groundStruct.color}");
            foreach (var item in groundStruct.drop_items)
            {
                Debug.Log($"Drop Item: {item}");
            }

        
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
