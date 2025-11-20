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
            // �ü� ������ �׼� ����
            for (int i = 0; i < intensity; i++)
            {
                facilityEntity.Logger("Hello from Facility! Intensity: " + intensity);
            }
            // ��: �ڿ� ����, ���? ��ȭ ��
        }
    }

    public class DrillFacilityAction : IFacilityAction
    {
        GroundComponent GroundComponent;
        public void SetGroundComponent(GroundComponent groundComponent)
        {
            GroundComponent = groundComponent;
        }
        public void ActivateFacility(FacilityEntity facilityEntity, int intensity)
        {
            GroundComponent.GroundEntity.GiveDamage(intensity);
        }
    }
    
}
