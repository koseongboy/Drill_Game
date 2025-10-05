using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DrillGame.Data
{
    public struct Ground_Structure
    {
        public int id;
        public int start_depth;
        public int end_depth;
        public int hp;
        public List<string> drop_items;
        public string color;
    }
    public class Ground_Data
    {
        #region Fields & Properties
        private Dictionary<int, Ground_Structure> groundTable = new Dictionary<int, Ground_Structure>();
        int numCol = 6;
        #endregion

        #region Singleton & initialization
        private Ground_Data() { }

        #endregion

        #region getters & setters
        public Ground_Structure GetGround_Structure(int id)
        {
            return groundTable[id];
        }
        public Dictionary<int, Ground_Structure> GetGroundTable() { return groundTable; }
        #endregion

        #region public methods
        public static async Task<Ground_Data> CreateAsync()
        {
            var parser = new Ground_Data();
            TextAsset csvData = await Addressables.LoadAssetAsync<TextAsset>("Ground_Data").Task;
            if (csvData != null)
            {
                parser.Parse(csvData.text);
                Addressables.Release(csvData);
                Debug.Log("Ground_Data CSV loaded and parsed successfully.");
            }
            else
            {
                Debug.LogError("Failed to load Ground_Data CSV.");
            }

            return parser;
        }



        #endregion

        #region private methods
        private void Parse(string csvText)
        {
            string[] lines = csvText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = line.Split('|');
                if (fields.Length == numCol)
                {
                    Ground_Structure structure = new Ground_Structure
                    {
                        id = int.Parse(fields[0]),
                        start_depth = int.Parse(fields[1]),
                        end_depth = int.Parse(fields[2]),
                        hp = int.Parse(fields[3]),
                        color = fields[5]
                    };
                    string[] items = fields[4].Split(';');
                    structure.drop_items = new List<string>();
                    foreach (var item in items)
                    {
                        structure.drop_items.Add(item);
                    }
                    groundTable[structure.id] = structure;
                }
                else
                {
                    Debug.LogWarning($"numCol 수정했나요??");
                }
            }


        }
        #endregion

        #region Unity event methods
        #endregion

    }
}
