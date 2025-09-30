using UnityEngine;
using DrillGame.Entity.Engine;
using System;

namespace DrillGame.Components
{
    public class CSV_Loader_forDEV : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        async void Start()
        {
            Engine_Data engineData;
            engineData = await Engine_Data.CreateAsync();
            Tuple<int, int>[] coords = engineData.GetCoordinate(12);
            Debug.Log("Coordinates for Engine ID 12:");
            foreach (var coord in coords)
            {
                Debug.Log($"Coordinate: ({coord.Item1}, {coord.Item2})");
            }
            coords = engineData.GetCoordinate(14);
            Debug.Log("Coordinates for Engine ID 14:");
            foreach (var coord in coords)
            {
                Debug.Log($"Coordinate: ({coord.Item1}, {coord.Item2})");
            }
            coords = engineData.GetCoordinate(24);
            Debug.Log("Coordinates for Engine ID 24:");
            foreach (var coord in coords)
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
