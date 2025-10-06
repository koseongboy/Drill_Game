using UnityEngine;

namespace DrillGame.Core.Facility
{
    public interface IFacilityAction
    {
        void ActivateFacility(int intensity);
    }

    public class HelloFacilityAction : IFacilityAction
    {
        public void ActivateFacility(int intensity)
        {
            // 시설 고유의 액션 구현
            Debug.Log($"Facility activated with intensity: {intensity}");
            // 예: 자원 생산, 방어 강화 등
        }
    }
}
