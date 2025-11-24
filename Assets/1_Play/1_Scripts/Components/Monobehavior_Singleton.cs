using UnityEngine;

namespace DrillGame._1_Play._1_Scripts.Components
{
    public class Monobehavior_Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindAnyObjectByType<T>();

                    // 2. 만약 씬에 없다면 새로 생성 (옵션)
                    if (!instance)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return instance;
            }
        }
    
        // 이 메서드가 Awake에서 호출되어, instance를 확실히 설정함
        protected virtual void Awake() 
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                // 중복 인스턴스 파괴
                Destroy(gameObject);
            }
        }
    }
}