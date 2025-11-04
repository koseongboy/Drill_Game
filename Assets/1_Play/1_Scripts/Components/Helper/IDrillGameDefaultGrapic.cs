using UnityEngine;

namespace DrillGame.View.Helper
{
    public interface IDrillGameDefaultGrapic
    {
        // 선택되었을 때 그래픽 변화
        void ChosenGraphic();

        // 초기 상태로 리셋
        void DefaultGraphic();
    }
}
