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
            Engine_Data engineData = await Engine_Data.CreateAsync();
            var coords = engineData.GetCoordinate("A-3");
            foreach(var coord in coords)
            {
                Debug.Log($"Coordinate: ({coord.Item1}, {coord.Item2})");
            }

        
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
