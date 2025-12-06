using System.Collections;
using System.Collections.Generic;
using DrillGame.Managers;
using DrillGame.View.Ground;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DrillGame.View.Drill
{
    public class DrillComponent : MonoBehaviour
    {
        #region Fields & Properties
        SpriteRenderer spriteRenderer;
        Drill_Data_ Currentdata;
        Sprite CurrentSprite;
        public ParticleSystem particle;

        private int drillLevel;

        // 흔들림 애니메이션 설정
        [Header("Wiggle 설정")]
        [SerializeField] private float wiggleDuration = 0.5f;   // 전체 흔들림에 걸리는 시간
        [SerializeField] private float wiggleMagnitude = 0.1f;  // 최대 좌우 이동 거리 (유닛)
        [SerializeField] private int wiggleCycles = 3;          // 총 왕복 횟수 (예: 3이면 왼쪽-오른쪽-왼쪽-오른쪽 총 3번 왕복)

        private Vector3 originalLocalPosition;
        #endregion

        #region Singleton & initialization
        private static DrillComponent instance;
        public static DrillComponent Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<DrillComponent>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            originalLocalPosition = transform.localPosition;
            Init();
        }

        private void Init()
        {
            
            spriteRenderer = GetComponent<SpriteRenderer>();

            drillLevel = SaveManager.Instance.LoadDrillData(1);
            Currentdata = ScriptableObjectManager.Instance.GetData<Drill_Data_>(drillLevel + 3000);
            Debug.Log("드릴 컴포넌트 초기화. 현재 레벨: " + drillLevel);

            if(Currentdata != null)
                Debug.Log("드릴 데이터 로드 완료. 현재 드릴 ID: " + Currentdata.Id);
            else
                Debug.LogError("드릴 데이터 로드 실패!");
            
            //드릴 스프라이트 초기화
            LoadSprite();
        }
        #endregion

        #region getters & setters
        public int GetDrillLevel()
        {
            return drillLevel;
        }
        public int GetDrillDamage()
        {
            return Currentdata.Damage;
        }
        
        #endregion

        #region public methods
        public async void LoadSprite()
        {

            Debug.Log("드릴 스프라이트 로드 시작. 현재 드릴 어드레서블 주소: " + Currentdata.DrillSprite);
            var currentHandle = Addressables.LoadAssetAsync<Sprite>(Currentdata.DrillSprite);
            CurrentSprite = await currentHandle.Task;
            spriteRenderer.sprite = CurrentSprite;
        }

        public void levelUp(int toWhat)
        {
            try
            {
                Drill_Data_ temp = ScriptableObjectManager.Instance.GetData<Drill_Data_>(3000 + toWhat);
                Currentdata = temp;
            } catch
            {
                Debug.Log("그런 레벨은 존재하지 않습니다. 아무 행동도 취하지 않습니다.");
                return;
            }
            drillLevel = toWhat;
            LoadSprite();
            SaveDrillData(drillLevel);
            Debug.Log("드릴 레벨 업! 현재 레벨: " + drillLevel);
        }

        public void oneLevelUp()
        {
            levelUp(drillLevel + 1);
        }
        

        public void resetData()
        {
            SaveManager.Instance.DeleteDrillData();
        }

        public void RunDrillAnimation()
        {
            StartCoroutine(DrillAnimation());
            
        }
        #endregion

        #region private methods
        private void SaveDrillData(int level)
        {
            SaveManager.Instance.SaveDrillData(level);
        }
        IEnumerator DrillAnimation()
        {
            
            // 1. 초기 위치로 돌아가도록 보장합니다.
            transform.localPosition = originalLocalPosition;
        
            // 2. 한 왕복(Wiggle) 사이클에 걸리는 시간 계산
            // (전체 시간 / (왕복 횟수 * 2)) => 1회 이동(예: 왼쪽으로)에 걸리는 시간
            float singleMoveDuration = wiggleDuration / (wiggleCycles * 2f);
            float elapsedTime = 0f;
            
            // 3. 총 왕복 횟수만큼 반복합니다.
            for (int i = 0; i < wiggleCycles * 2; i++)
            {
                elapsedTime = 0f;
                particle.Play();
                // 목표 위치 (홀수 번째 이동은 오른쪽, 짝수 번째 이동은 왼쪽)
                // 시작이 0번(짝수)이므로 첫 이동은 Left(-Magnitude)입니다.
                Vector3 targetPosition;
                if (i % 2 == 0) // 0, 2, 4번째 (왼쪽/음수 방향으로 이동)
                {
                    targetPosition = originalLocalPosition + Vector3.left * wiggleMagnitude;
                }
                else // 1, 3, 5번째 (오른쪽/양수 방향으로 이동)
                {
                    targetPosition = originalLocalPosition + Vector3.right * wiggleMagnitude;
                }
                
                // 현재 위치 (이전 목표 위치)
                Vector3 startPosition = transform.localPosition;

                // 4. 단일 이동 애니메이션 (Lerp를 사용한 부드러운 이동)
                while (elapsedTime < singleMoveDuration)
                {
                    // 시간에 비례하여 위치를 보간(Lerp)합니다.
                    transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / singleMoveDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null; // 다음 프레임까지 대기
                }
                
                // 목표 위치에 정확히 도달하도록 보정
                transform.localPosition = targetPosition;
            }

            // 5. 최종적으로 원래 위치로 부드럽게 복귀합니다.
            elapsedTime = 0f;
            Vector3 finalStartPosition = transform.localPosition;
        
            // 복귀 시간은 총 Wiggle 시간의 1/4 정도를 사용합니다.
            float returnDuration = wiggleDuration * 0.25f; 

            while (elapsedTime < returnDuration)
            {
                transform.localPosition = Vector3.Lerp(finalStartPosition, originalLocalPosition, elapsedTime / returnDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 최종 위치 보정
            transform.localPosition = originalLocalPosition;
            Debug.Log("Wiggle 애니메이션 종료.");
        }
        #endregion

        #region Unity event methods

        private void Start()
        {
        
        }

        private void Update()
        {
        
        }
        #endregion
    }
}