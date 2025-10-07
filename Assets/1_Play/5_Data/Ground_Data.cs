using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DrillGame.Data
{
    public struct Ground_Structure
    {

        public int start_depth;
        public int end_depth;
        public int hp;
        public List<string> drop_items;
        public string sprite_addressable;
    }
    public class Ground_Data
    {
        #region Fields & Properties
        public Dictionary<int, Ground_Structure> GroundTable { get; private set; } = new Dictionary<int, Ground_Structure>();
        public List<int> DepthRanges {get; set;} = new List<int>();
        int numCol = 5;
        #endregion

        #region Singleton & initialization
        private Ground_Data() { }

        #endregion

        #region getters & setters
        
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
                        start_depth = int.Parse(fields[0]),
                        end_depth = int.Parse(fields[1]),
                        hp = int.Parse(fields[2]),
                        sprite_addressable = fields[4]
                    };
                    string[] items = fields[3].Split(';');
                    structure.drop_items = new List<string>();
                    foreach (var item in items)
                    {
                        structure.drop_items.Add(item);
                    }
                    GroundTable[structure.start_depth] = structure;
                    DepthRanges.Add(structure.start_depth);
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
