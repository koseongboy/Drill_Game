using DrillGame.Core.Ground;
using DrillGame.View.Ground;
using UnityEngine;


namespace DrillGame.Core.Facility
{
    
    public interface IFacilityAction
    {
        void ActivateFacility(FacilityEntity facilityEntity, int intensity, int level);
    }

    public class HelloFacilityAction : IFacilityAction
    {
        public void ActivateFacility(FacilityEntity facilityEntity, int intensity, int level)
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
        public void ActivateFacility(FacilityEntity facilityEntity, int intensity, int level)
        {
            
            for (int i = 0; i < intensity; i++)
            {
                GroundComponent.Instance.GroundEntity.GiveDamage(level * 10);
                Debug.Log("level: " + level);
                Debug.Log("땅에 " + (intensity * 10) + " 만큼 데미지를 줌 (남은 땅의 체력: " + GroundComponent.Instance.GroundEntity.CurrentHp + ")");
            }
            
            
        }

        public void ActivateFacility(FacilityEntity facilityEntity, int intensity, GroundComponent groundComponent, int drillDamage)
        {
            for (int i = 0; i < intensity; i++)
            {
                groundComponent.GroundEntity.GiveDamage(drillDamage);
                Debug.Log("땅에 " + drillDamage + " 만큼 데미지를 줌 (남은 땅의 체력: " + groundComponent.GroundEntity.CurrentHp + ")");
                if (groundComponent.GroundEntity.IsDestroyed)
                {
                    Debug.Log("땅이 파괴됨!");
                }
            }
        }


    }
}
