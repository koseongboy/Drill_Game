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
            // ì‹œì„¤ ê³ ìœ ì˜ ì•¡ì…˜ êµ¬í˜„
            for (int i = 0; i < intensity; i++)
            {
                facilityEntity.Logger("Hello from Facility! Intensity: " + intensity);
            }
            // ì˜ˆ: ìì› ìƒì‚°, ë°©ì–´ ê°•í™” ë“±
        }
    }

    public class DrillAction : IFacilityAction
    {
        public void ActivateFacility(FacilityEntity facilityEntity, int intensity)
        {
            Debug.LogError("µå¸±Àº ¿À¹ö·ÎµåµÈ ´Ù¸¥ ¸Ş¼Òµå¸¦ È£ÃâÇÏ¼¼¿ä.");
        }

        public void ActivateFacility(FacilityEntity facilityEntity, int intensity, GroundComponent groundComponent, int drillDamage)
        {
            for (int i = 0; i < intensity; i++)
            {
                groundComponent.GroundEntity.GiveDamage(drillDamage);
                Debug.Log("µå¸±ÀÌ ¶¥¿¡ " + drillDamage + " µ¥¹ÌÁö¸¦ ÀÔÇû½À´Ï´Ù. (³²Àº Ã¼·Â: " + groundComponent.GroundEntity.CurrentHp + ")");
                if (groundComponent.GroundEntity.IsDestroyed)
                {
                    Debug.Log("¶¥ÀÌ ÆÄ±«µÇ¾ú½À´Ï´Ù!");
                }
            }
        }


    }
}
