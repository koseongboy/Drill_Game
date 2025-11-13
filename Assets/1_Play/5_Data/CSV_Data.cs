using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace DrillGame.Data
{

    public class CSV_Data
    {
        #region Fields & Properties
        // 데이터 구조를 Dictionary<string, Dictionary<string, string>>으로 변경
        public Dictionary<string, Dictionary<string, string>> Table {get; protected set; } = new Dictionary<string, Dictionary<string, string>>();

        // numCol은 더 이상 필요 없습니다.
        #endregion

        #region Singleton & initialization
        protected CSV_Data() { }
        #endregion

        #region getters & setters
        public string Get(string id, string columnName)
        {
            if (Table.TryGetValue(id, out var rowData))
            {
                if (rowData.TryGetValue(columnName, out var value))
                {
                    return value;
                }
                else
                {
                    Debug.LogError($"Column '{columnName}' not found for ID '{id}'.");
                    return null;
                }
            }
            else
            {
                Debug.LogError($"ID '{id}' not found in the data.");
                return null;
            }
        }
        
        #endregion

        #region public methods
        public static async Task<CSV_Data> CreateAsync(string addressableName)
        {
            var parser = new CSV_Data();
            TextAsset csvData = await Addressables.LoadAssetAsync<TextAsset>(addressableName).Task;
            if (csvData != null)
            {
                parser.Parse(csvData.text);
                Addressables.Release(csvData);
                Debug.Log("Engine_Data CSV loaded and parsed successfully.");
            }
            else
            {
                Debug.LogError($"Failed to load {addressableName} CSV.");
            }
            return parser;
        }
        #endregion

        #region private methods
        // Parse 메서드를 동적으로 작동하도록 완전히 재작성
        private void Parse(string csvText)
        {
            Table.Clear();
            string[] lines = csvText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 2) // 헤더와 데이터가 최소 1줄씩은 있어야 함
            {
                Debug.LogError("CSV 데이터는 최소 2줄(헤더 + 데이터)을 포함해야 합니다.");
                return;
            }

            // 1. 첫 번째 줄을 헤더로 사용 (구분자: '|')
            string[] headers = lines[0].Split('|');
            for(int i = 0; i < headers.Length; i++)
            {
                headers[i] = headers[i].Trim(); // 헤더 공백 제거
            }

            // 2. 두 번째 줄부터 데이터로 처리
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split('|');
                if (fields.Length != headers.Length)
                {
                    Debug.LogWarning($"Line {i+1} has incorrect number of columns. Skipping.");
                    continue;
                }
                
                var rowData = new Dictionary<string, string>();
                for (int j = 0; j < headers.Length; j++)
                {
                    // 헤더를 Key로, 필드를 Value로 딕셔너리에 추가
                    rowData[headers[j]] = fields[j].Trim();
                }

                // "id" 컬럼의 값을 메인 딕셔너리의 Key로 사용
                if (rowData.ContainsKey("ID"))
                {
                    Table[rowData["ID"]] = rowData;
                }
                else
                {
                    Table[rowData[headers[0]]] = rowData; // "ID" 컬럼이 없으면 첫 번째 컬럼을 Key로 사용
                }
            }
        }
        #endregion
    }
}