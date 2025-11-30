using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.View.Engine
{
    public class WireComponent : MonoBehaviour
    {
        #region Fields & Properties
        [SerializeField]
        Color wireColor;

        [ReadOnly]
        [SerializeField]
        GameObject wirePrefab;

        List<GameObject> wireList = new();




        #endregion

        #region Singleton & initialization
        private void Awake()
        {
            Debug.Log("0");
            wirePrefab = Resources.Load<GameObject>("Prefabs/etc/Wire");

            Debug.Log("1");
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void SetWire(Vector2 enginePos, Vector2 corePos)
        {
            Debug.Log("2");
            // test 직선 설치
            Debug.Log("도비가 와이어를 설치합니다.");

            int length = (int)(corePos.x - enginePos.x);
            int width = (int)(corePos.y - enginePos.y);

            for (int i = 0; i <= length; i++)
            {
                GameObject wire = Instantiate(wirePrefab, transform);
                wireList.Add(wire);

                wire.transform.position = new Vector2(enginePos.x + i, enginePos.y);
                SpriteRenderer sr = wire.GetComponent<SpriteRenderer>();
                sr.color = wireColor;
            }
            for (int j = 0; j < width; j++)
            {
                GameObject wire = Instantiate(wirePrefab, transform);
                wireList.Add(wire);

                wire.transform.position = new Vector2(corePos.x, enginePos.y + j);
                SpriteRenderer sr = wire.GetComponent<SpriteRenderer>();
                //sr.color = wireColor;


            }
        }

        public void Activate(int index)
        {
            
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}
