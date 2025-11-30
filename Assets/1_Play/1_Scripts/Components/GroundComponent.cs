using System;
using System.Collections;
using System.Threading.Tasks;
using DrillGame.Core.Ground;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DrillGame.View.Ground
{
    public class GroundComponent : MonoBehaviour
    {
        #region Fields & Properties
        public GroundEntity GroundEntity { get; private set; }
        private SpriteRenderer spriteRenderer;

        private const string ES3FILENAME = "GroundUserData.es3";
        private const string GROUND_DEPTH = "GroundDepth";
        private const string GROUND_HP = "GroundHP";
        
        private Ground_Data_ CurrentGroundData;
        private AsyncOperationHandle CurrentGroundHandle;
        private AsyncOperationHandle NextGroundHandle;
        private Sprite CurrentGroundSprite;
        private Sprite NextGroundSprite;

        private int currentDepth = 0;
        public int depthIncrement = 1; //땅 파괴 시 증가하는 깊이 (임시)
        
        public event Action<int> OnDepthChanged;
        public event Action<int> OnHpChanged;
        
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

        private static GroundComponent instance;
        public static GroundComponent Instance
        {
            get
            {
                // 아직 인스턴스가 없다면, 씬에서 찾아 초기화 시도
                if (instance == null)
                {
                    instance = FindAnyObjectByType<GroundComponent>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            //엔티티 생성
            GroundEntity = new GroundEntity();
            
            spriteRenderer = GetComponent<SpriteRenderer>();
            ES3File es3File = new ES3File(ES3FILENAME);
            currentDepth = es3File.Load(GROUND_DEPTH, 1);
            int userHp = es3File.Load(GROUND_HP, ScriptableObjectManager.Instance.GetData<Ground_Data_>(5001).HP);
            CurrentGroundData = ScriptableObjectManager.Instance.GetData<Ground_Data_>( getGroundDataKey_ByDepth(currentDepth) );
            
            //기존 데이터로 엔티티 및 땅 색(재질) 초기화
            setNewData(currentDepth, userHp);
        }

        #endregion

        #region getters & setters

        public int GetDepth()
        {
            return currentDepth;
        }
        #endregion

        #region public methods
        public void GiveDamage(int damage)
        {
            GroundEntity.GiveEntityDamage(damage);
            OnHpChanged?.Invoke( GroundEntity.CurrentHp );
            Debug.Log("땅에 "+ damage +" 데미지 입힘 (남은 체력: " + GroundEntity.CurrentHp + ")");
            if (GroundEntity.IsDestroyed)
            {
                Debug.Log("땅 파괴됨!");
                setNewData(GroundEntity.Depth + depthIncrement);
            }
        }
        #endregion

        #region private methods

        private int getGroundDataKey_ByDepth(int depth)
        {
            // GroundData는 5001에서 시작해서, 5단계마다 1씩 증가함.
            int range = depth / 5;
            if (range > 3) range = 3; //개발용 최대 4까지만. (최대깊이 -1로 설정했을 때 로직이 망가져있음 todo.)
            return 5001 + range;
        }
        
        //입력받는 값에 따라 엔티티 세팅 (깊이만 줬을 때 = 새로운 땅 생성할 때)
        private void setNewData(int depth)
        {
            // Debug.Log("새 땅이 생성되었습니다. 깊이: " + depth);
            currentDepth = depth;
            CurrentGroundData = ScriptableObjectManager.Instance.GetData<Ground_Data_>( getGroundDataKey_ByDepth(depth) );
            
            GroundEntity.SetInformation(depth, CurrentGroundData.HP, CurrentGroundData.HP, CurrentGroundData.DropItems);
            OnDepthChanged?.Invoke( GroundEntity.Depth );
            OnHpChanged?.Invoke( GroundEntity.CurrentHp );
            StartCoroutine(AppearAnimation());
            if (depth == CurrentGroundData.StartDepth) //구간에 처음 진입했을 경우
            {
                CurrentGroundSprite = NextGroundSprite; // 땅의 스프라이트를 갈아끼워줌.
                LoadGroundSpriteAsync(CurrentGroundData.SpriteAddressable);
            }
            spriteRenderer.sprite = CurrentGroundSprite;
        }
        //입력받는 값에 따라 엔티티 세팅 (hp도 줬을 때 = 기존 유저 데이터 불러올 때)
        private void setNewData(int depth, int hp)
        {
            // Debug.Log("<<게임 시작>> \n 새 땅이 생성되었습니다. 깊이: " + depth);
            currentDepth = depth;
            CurrentGroundData = ScriptableObjectManager.Instance.GetData<Ground_Data_>( getGroundDataKey_ByDepth(depth) );
            GroundEntity.SetInformation(depth, hp, CurrentGroundData.HP, CurrentGroundData.DropItems);
            OnDepthChanged?.Invoke( GroundEntity.Depth );
            OnHpChanged?.Invoke( GroundEntity.CurrentHp );
            if (CurrentGroundSprite == null)
            {
                LoadGroundSpriteAsync_OnGameStart(
                    CurrentGroundData.SpriteAddressable,
                    ScriptableObjectManager.Instance.GetData<Ground_Data_>(getGroundDataKey_ByDepth(depth + 1))
                        .SpriteAddressable);
            }
            StartCoroutine(AppearAnimation()); // TODO : DOTween으로 바꾸기
        }

        private async void LoadGroundSpriteAsync_OnGameStart(string currentSpriteName, string nextSpriteName)
        {
            CurrentGroundHandle = Addressables.LoadAssetAsync<Sprite>(currentSpriteName);
            NextGroundHandle = Addressables.LoadAssetAsync<Sprite>(nextSpriteName);
            
            CurrentGroundSprite = (Sprite)await CurrentGroundHandle.Task;
            NextGroundSprite = (Sprite)await NextGroundHandle.Task;
            
            spriteRenderer.sprite = CurrentGroundSprite;
        }

        private async void LoadGroundSpriteAsync(string nextSpriteName)
        {
            Addressables.Release(CurrentGroundHandle);
            CurrentGroundHandle = NextGroundHandle;
            NextGroundHandle = Addressables.LoadAssetAsync<Sprite>(nextSpriteName);

            await Task.WhenAll(NextGroundHandle.Task);
            NextGroundSprite = (Sprite)NextGroundHandle.Result;
        }
        
        private void SaveCurrentGroundData(int depth, int hp)
        {
            ES3File es3File = new ES3File(ES3FILENAME);
            es3File.Save(GROUND_DEPTH, depth);
            es3File.Save(GROUND_HP, hp);
            es3File.Sync();
        }

        #endregion

        #region Unity event methods
        private void OnApplicationQuit()
        {
            SaveCurrentGroundData(GroundEntity.Depth, GroundEntity.CurrentHp);
        }
        #endregion
        
        #region DEV

        [ContextMenu("Reset Saved Depth & HP")]
        private void ResetSavedDepth_DEV()
        {
            ES3.DeleteFile(ES3FILENAME);
            Init();
            Debug.Log("Ground UserData가 성공적으로 초기화되었습니다.");
        }
        #endregion
    }
}