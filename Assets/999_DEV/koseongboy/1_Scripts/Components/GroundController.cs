using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using DrillGame.Managers;

namespace DrillGame.Components
{
    public class GroundController : MonoBehaviour
    {


        #region Fields & Properties
        //애니메이션 관련
        public float appearDuration = 0.3f;
        public float appearOffset = 1.0f;
        IEnumerator AppearAnimation()
        {
            Vector2 targetPosition = transform.position;
            Vector2 startPosition = targetPosition - Vector2.up * appearOffset;

            transform.position = startPosition;
            float elapsedTime = 0f;

            while (elapsedTime < appearDuration)
            {
                transform.position = Vector2.Lerp(startPosition, targetPosition, (elapsedTime / appearDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        //땅 관련
        private int depth;
        private int hp = 5;
        //매니져

        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        public int GetHp() { return hp; }
        #endregion

        #region public methods
        public bool TakeDamage(int damage)
        {
            hp -= damage;
            Debug.Log("give damage " + damage + " (남은 채력: " + hp + ")");
            if (hp <= 0)
            {
                return true;
            }
            else return false;
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        void Start()
        {

        }


        void Update()
        {

        }

        void OnEnable()
        {
            StartCoroutine(AppearAnimation());
        }
        void OnDestroy()
        {
            
            
        }
        #endregion

  }
}
