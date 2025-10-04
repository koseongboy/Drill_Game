using UnityEngine;

namespace DrillGame.GameSetting
{
    [ExecuteInEditMode]
    public class FixedHorizontalCamera : MonoBehaviour
    {
        // 원하는 화면 가로 길이 (유니티 유닛 기준. 타일 8칸 + 여유 1 + 1)
        [SerializeField]
        private float targetHorizontalSize = 10.0f;

        private Camera cam;

        void Awake()
        {
            cam = GetComponent<Camera>();
            if (cam == null || !cam.orthographic)
            {
                Debug.LogError("이 스크립트는 Orthographic Camera에 부착되어야 합니다.");
                // 에디터 모드에서는 실수로 비활성화되지 않도록 주의
            }
        }

        // 런타임 시작 시 한 번 실행
        void Start()
        {
            // 에디터 모드와 런타임 모두에서 작동
            AdjustOrthographicSize();
        }

        // (선택 사항: Inspector에서 targetHorizontalSize 값 변경 시 즉시 반영)
        void OnValidate()
        {
            if (cam != null)
            {
                AdjustOrthographicSize();
            }
            else
            {
                Debug.LogWarning("Camera 컴포넌트를 찾을 수 없습니다. Awake()에서 초기화가 필요합니다.");
            }
        }

        // 해상도 변경 시 카메라 사이즈를 조정하는 핵심 함수
        void AdjustOrthographicSize()
        {
            // 에디터에서는 Game View의 크기가 Screen 클래스가 아닌 Camera.aspect로 결정됩니다.
            // Screen.width/height는 실행 파일의 최종 해상도를 따르지만,
            // 에디터에서는 카메라의 현재 종횡비(aspect)를 사용해야 정확합니다.

            float currentAspect = cam.aspect; // 현재 카메라의 종횡비를 사용

            float halfTargetWidth = targetHorizontalSize / 2.0f;

            float newOrthographicSize = halfTargetWidth / currentAspect;

            cam.orthographicSize = newOrthographicSize;
        }
    }
}
