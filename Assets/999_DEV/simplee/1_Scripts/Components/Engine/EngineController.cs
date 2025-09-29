using DG.Tweening;
using DrillGame.Entity.Engine;
using UnityEngine;

namespace DrillGame.Components.Engine
{
    public class EngineController : MonoBehaviour
    {
        #region Fields & Properties
        private Engine_Base engine;
        [SerializeField]
        [Tooltip("debug 용도로 엔진의 이름을 직접 작성할 수 있습니다. 실 사용시 비워두세요")]
        private string engineName;

        #endregion

        #region Singleton & initialization
        public void Initialize(Engine_Base engine)
        {
            this.engine = engine;
            engineName = engine.GetType().Name;
            engine.OnActivatedEvent += HandleEngineActivated;
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        
        #endregion

        #region private methods
        private void HandleEngineActivated()
        {
            Debug.Log($"mono - {engineName} 엔진이 active 되었습니다.");
            TempGraphicAction();
        }

        private void TempGraphicAction()
        {
            // Vector3.one * 0.2f는 모든 축으로 0.2만큼 펀치한다는 의미입니다.
            transform.DOPunchScale(
                punch: Vector3.one * 0.2f, // 현재 크기에서 20% 커짐
                duration: 0.2f,             // 0.2초 동안 커졌다 돌아옴
                vibrato: 1,                 // 한 번만 튀어나왔다 돌아오도록 설정
                elasticity: 1               // 탄성력 설정 (1이 일반적으로 부드러움)
            );
        }
        #endregion

        #region Unity event methods

        private void Start()
        {
            if (engineName == null)
            {
                Debug.LogWarning("EngineController에 엔진이 할당되지 않았습니다. 디버그 중이라면 name을 채우시고, 실 개발 중이라면 생성 코드 뒤에 Initialize를 해주세요");
            }
            else
            {
                if (engineName == "Normal")
                {
                    Engine_Normal normalEngine = new Engine_Normal();
                    Initialize(normalEngine);
                }
                else
                {
                    Debug.LogError("잘못된 string 값을 입력해서 디버그용 엔진 class를 불러올 수 없습니다.");
                }
            }
        }

        
        #endregion
    }
}
