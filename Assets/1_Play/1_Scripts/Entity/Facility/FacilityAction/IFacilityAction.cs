using DrillGame.Core.Ground;
using DrillGame.View.Ground;
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

    public class DrillAction : IFacilityAction
    {
        public void ActivateFacility(FacilityEntity facilityEntity, int intensity)
        {
            Debug.LogError("드릴은 오버로드된 다른 메소드를 호출하세요.");
        }

        public void ActivateFacility(FacilityEntity facilityEntity, int intensity, GroundComponent groundComponent, int drillDamage)
        {
            for (int i = 0; i < intensity; i++)
            {
                groundComponent.GroundEntity.GiveDamage(drillDamage);
                Debug.Log("드릴이 땅에 " + drillDamage + " 데미지를 입혔습니다. (남은 체력: " + groundComponent.GroundEntity.CurrentHp + ")");
                if (groundComponent.GroundEntity.IsDestroyed)
                {
                    Debug.Log("땅이 파괴되었습니다!");
                }
            }
        }


    }
}
