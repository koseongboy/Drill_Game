using System.Collections;
using System.Collections.Generic;
using DrillGame.Core.Ground;
using DrillGame.Data;
using DrillGame.Managers;
using UnityEngine;

namespace DrillGame.View.Ground
{
    public class GroundComponent : MonoBehaviour
    {
        //test
        public void OnButtonClick()
        {
            if (GroundEntity.GiveDamage(1))
            {
                Debug.Log("땅 파괴됨!");
                setNewData(GroundEntity.GetNextDepth());
            }
            Debug.Log("땅에 1 데미지 입힘 (남은 체력: " + GroundEntity.CurrentHp + ")");
        }
        #region Fields & Properties
        public GroundEntity GroundEntity { get; private set; }
        public Dictionary<int, Ground_Structure> GroundTable => DataLoadManager.Instance.GroundTable;
        private SpriteRenderer spriteRenderer;

        [SerializeField]private List<Sprite> groundSprites = new List<Sprite>();

        //애니메이션 관련
        public float appearDuration = 0.3f;
        public float appearOffset = 1.0f;
        IEnumerator AppearAnimation()
        {
            Vector2 targetPosition = transform.position;
            Vector2 startPosition = targetPosition - Vector2.up * appearOffset;

            transform.position = startPosition;
            float elapsedTime = 0f;

            while (elapsedTime < appearDuration)
            {
                transform.position = Vector2.Lerp(startPosition, targetPosition, (elapsedTime / appearDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        #endregion

        #region public methods

        #endregion

        #region private methods
        //입력받는 값에 따라 엔티티 세팅 (깊이만 줬을 때 <=> 새로운 땅 생성할 때, hp도 줬을 때 <=> 기존 유저 데이터 불러올 때)
        private void setNewData(int depth, int hp = -1)
        {
            foreach (var groundIdx in GroundTable.Keys)
            {
                if (depth >= GroundTable[groundIdx].start_depth && depth <= GroundTable[groundIdx].end_depth)
                {
                    GroundEntity.SetInformation(depth, hp != -1 ? hp : GroundTable[groundIdx].hp, GroundTable[groundIdx].hp, GroundTable[groundIdx].drop_items);
                    spriteRenderer.sprite = groundSprites[groundIdx - 1];
                    StartCoroutine(AppearAnimation());
                    return;
                }
            }
            Debug.LogError("GroundComponent: 해당 depth에 맞는 땅 정보가 없습니다. depth:" + depth);
        }

        #endregion

        #region Unity event methods
        private void Awake()
        {

        }

        private void Start()
        {
            //엔티티 생성
            GroundEntity = new GroundEntity();
            spriteRenderer = GetComponent<SpriteRenderer>();
            //기존 데이터 로딩 (개발용 임시) -> User Data Save/Load 기능 구현 후 수정 (test)
            DataLoadManager.Instance.UserData.TryGetValue("Ground", out List<string> userData);
            int user_depth = int.Parse(userData[0]);
            int user_hp = int.Parse(userData[1]);
            //기존 데이터로 엔티티 및 땅 색(재질) 초기화
            setNewData(user_depth, user_hp);

        }

        private void Update()
        {
        
        }
        #endregion
    }
}