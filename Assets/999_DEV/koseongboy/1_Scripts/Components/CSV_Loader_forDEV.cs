using UnityEngine;
using DrillGame.Data;
using System;
using UnityEngine.SceneManagement;

namespace DrillGame.Components
{
    public class CSV_Loader_forDEV : MonoBehaviour
    {
        Engine_Data engineData;
        Facility_Data facilityData;
        Ground_Data groundData;
        async void Start()
        {
            Debug.Log("CSV 데이터 로딩 시작..");
            engineData = await Engine_Data.CreateAsync();
            facilityData = await Facility_Data.CreateAsync();
            groundData = await Ground_Data.CreateAsync();
            Debug.Log("CSV 데이터 로딩 완료.");
            SceneManager.LoadScene("Play_koseongboy");
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
