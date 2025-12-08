using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.View.Engine
{
    public class WireComponent : MonoBehaviour
    {
        #region Fields & Properties

        
        [SerializeField]
        GameObject wirePrefab;
        [SerializeField]
        GameObject wireWidthPrefab;

        List<GameObject> wireList = new();

        [SerializeField]
        Color setColor;


        #endregion

        #region Singleton & initialization
        private void Awake()
        {
            

        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void SetWire(Vector2 enginePos, Vector2 corePos)
        {
            Debug.Log("도비가 와이어를 설치합니다.");


            // 1. 거리(절댓값) 계산: 몇 칸을 설치해야 하는지 (음수가 나오지 않게 Mathf.Abs 사용)
            int xDistance = (int)Mathf.Abs(corePos.x - enginePos.x);
            int yDistance = (int)Mathf.Abs(corePos.y - enginePos.y);

            // 2. 방향 계산: 어느 쪽으로 갈지 정함 (1이면 오른쪽/위, -1이면 왼쪽/아래)
            int xDirection = (corePos.x > enginePos.x) ? 1 : -1;
            int yDirection = (corePos.y > enginePos.y) ? 1 : -1;

            // 3. 가로 방향(X축) 설치
            for (int i = 0; i < xDistance; i++)
            {
                GameObject wire = Instantiate(wireWidthPrefab, transform);
                wireList.Add(wire);

                // 현재 위치에서 방향(xDirection)만큼 i번 이동
                float currentX = enginePos.x + (i * xDirection);
                wire.transform.position = new Vector2(currentX, enginePos.y);
            }
            // 4. 모서리 부분 설치
            GameObject cornerWire = Instantiate(wirePrefab, transform);
            wireList.Add(cornerWire);
            cornerWire.transform.position = new Vector2(corePos.x, enginePos.y);
            if (xDirection == 1 && yDirection == 1)
            {
                cornerWire.transform.rotation = Quaternion.Euler(0, 0, 180); // 왼쪽 아래
            }
            else if (xDirection == 1 && yDirection == -1)
            {
                cornerWire.transform.rotation = Quaternion.Euler(0, 0, 270); // 오른쪽 아래
            }
            else if (xDirection == -1 && yDirection == 1)
            {
                cornerWire.transform.rotation = Quaternion.Euler(0, 0, 90); // 왼쪽 위
            }
            else if (xDirection == -1 && yDirection == -1)
            {
                cornerWire.transform.rotation = Quaternion.Euler(0, 0, 0); // 오른쪽 위
            }



            // 5. 세로 방향(Y축) 설치
            // j를 1부터 시작하는 이유: 0부터 하면 꺾이는 모서리 부분에 와이어가 2개 겹쳐서 생성됨
            for (int j = 1; j <= yDistance; j++)
            {
                GameObject wire = Instantiate(wireWidthPrefab, transform);
                wireList.Add(wire);

                // X축 이동이 끝난 지점(corePos.x)에서 위/아래로 이동
                float currentY = enginePos.y + (j * yDirection);
                wire.transform.position = new Vector2(corePos.x, currentY);
                wire.transform.rotation = Quaternion.Euler(0, 0, 90); // 세로 방향으로 회전
            }
        }

        public void ActivateWire(int index)
        {
            // 와이어 활성화 애니메이션 (예: 색상 변경)
            wireList[index].GetComponentInChildren<SpriteRenderer>().DOColor(setColor, 0.2f).SetLoops(2, LoopType.Yoyo);
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}
