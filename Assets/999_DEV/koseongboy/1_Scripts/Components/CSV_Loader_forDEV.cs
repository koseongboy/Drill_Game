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
            Engine_Data engineData;
            engineData = await Engine_Data.CreateAsync();
            Tuple<int, int>[] coords = engineData.GetCoordinate("normal-2");
            Debug.Log("Coordinates for Engine ID normal-2:");
            foreach (var coord in coords)
            {
                Debug.Log($"Coordinate: ({coord.Item1}, {coord.Item2})");
            }
            coords = engineData.GetCoordinate("normal-4");
            Debug.Log("Coordinates for Engine ID normal-4:");
            foreach (var coord in coords)
            {
                Debug.Log($"Coordinate: ({coord.Item1}, {coord.Item2})");
            }
            coords = engineData.GetCoordinate("special-4");
            Debug.Log("Coordinates for Engine ID special-4:");
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
