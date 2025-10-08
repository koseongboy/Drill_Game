using UnityEngine;

namespace DrillGame.Core.Facility
{
    public interface IFacilityAction
    {
        void ActivateFacility(FacilityEntity facilityEntity , int intensity);
    }

    public class HelloFacilityAction : IFacilityAction
    {
        public void ActivateFacility(FacilityEntity facilityEntity, int intensity)
        {
            // 시설 고유의 액션 구현
            for (int i = 0; i < intensity; i++)
            {
                facilityEntity.Logger("Hello from Facility! Intensity: " + intensity);
            }
            // 예: 자원 생산, 방어 강화 등
        }
    }
}
