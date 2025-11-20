using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrillGame.Core.Ground;
using DrillGame.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DrillGame.View.Ground
{
    public class GroundComponent : MonoBehaviour
    {
        #region Fields & Properties
        public GroundEntity GroundEntity { get; private set; }
        public Dictionary<int, Dictionary<string, string>> GroundTable;
        private SpriteRenderer spriteRenderer;
        
        private Ground_Data_ CurrentGroundData;
        
        private AsyncOperationHandle CurrentGroundHandle;
        private AsyncOperationHandle NextGroundHandle;
        private Sprite CurrentGroundSprite;
        private Sprite NextGroundSprite;

        public int depthIncrement = 1; //땅 파괴 시 증가하는 깊이 (임시)

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

        private int getGroundDataKey_ByDepth(int depth)
        {
            // GroundData는 5001에서 시작해서, 5단계마다 1씩 증가함.
            int range = depth % 5;
            return 5001 + range;
        }
        
        //입력받는 값에 따라 엔티티 세팅 (깊이만 줬을 때 = 새로운 땅 생성할 때, hp도 줬을 때 = 기존 유저 데이터 불러올 때)
        private void setNewData(int depth)
        {
            // 여기 문제 생기면 김명준 잘못임
            // 여기 문제 생기면 김명준 잘못임
            // 여기 문제 생기면 김명준 잘못임

            Debug.Log("새 땅이 생성되었습니다. 깊이: " + depth);
            CurrentGroundData = ScriptableObjectManager.Instance.GetData<Ground_Data_>( getGroundDataKey_ByDepth(depth) );
            
            GroundEntity.SetInformation(depth, CurrentGroundData.HP, CurrentGroundData.HP, CurrentGroundData.DropItems);
            StartCoroutine(AppearAnimation());
            if (depth == CurrentGroundData.StartDepth) //구간에 처음 진입했을 경우
            {
                CurrentGroundSprite = NextGroundSprite; // 땅의 스프라이트를 갈아끼워줌.
                LoadGroundSpriteAsync(CurrentGroundData.SpriteAddressable);
            }

            spriteRenderer.sprite = CurrentGroundSprite;
        }
        private void setNewData(int depth, int hp)
        {
            // 여기 문제 생기면 김명준 잘못임
            // 여기 문제 생기면 김명준 잘못임
            // 여기 문제 생기면 김명준 잘못임
            
            Debug.Log("<<게임 시작>> \n 새 땅이 생성되었습니다. 깊이: " + depth);
            CurrentGroundData = ScriptableObjectManager.Instance.GetData<Ground_Data_>( getGroundDataKey_ByDepth(depth) );
            GroundEntity.SetInformation(depth, hp, CurrentGroundData.HP, CurrentGroundData.DropItems);
            spriteRenderer.sprite = CurrentGroundSprite;
            StartCoroutine(AppearAnimation()); // TODO : DOTween으로 바꾸기
        }

        private async void LoadGroundSpriteAsync(string nextSpriteName)
        {
            // 여기 문제 생기면 김명준 잘못임
            // 여기 문제 생기면 김명준 잘못임
            // 여기 문제 생기면 김명준 잘못임
            
            Addressables.Release(CurrentGroundHandle);
            CurrentGroundHandle = NextGroundHandle;
            NextGroundHandle = Addressables.LoadAssetAsync<Sprite>(nextSpriteName);

            await Task.WhenAll(NextGroundHandle.Task);
            NextGroundSprite = (Sprite)NextGroundHandle.Result;
        }

        #endregion

        #region Unity event methods
        private void Start()
        {
            //엔티티 생성
            GroundEntity = new GroundEntity();
            spriteRenderer = GetComponent<SpriteRenderer>();
            int user_depth = 3; // TODO
            int user_hp = 5; // TODO
            
            CurrentGroundData = ScriptableObjectManager.Instance.GetData<Ground_Data_>( getGroundDataKey_ByDepth(user_depth) );
            
            //기존 데이터로 엔티티 및 땅 색(재질) 초기화
            setNewData(user_depth, user_hp);
        }
        #endregion
        
        #region DEV
        public void OnButtonClick()
        {
            GroundEntity.GiveDamage(1);
            Debug.Log("땅에 1 데미지 입힘 (남은 체력: " + GroundEntity.CurrentHp + ")");
            if (GroundEntity.IsDestroyed)
            {
                Debug.Log("땅 파괴됨!");
                setNewData(GroundEntity.Depth + depthIncrement);
            }
        }
        #endregion
    }
}