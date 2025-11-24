namespace DrillGame._1_Play._1_Scripts.Components
{
    public class Singleton_CSharp<T> where T : Singleton_CSharp<T>, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                // 인스턴스가 null일 때만 새로 생성 (지연 초기화)
                if (instance == null)
                {
                    instance = new T();
                    instance.Init();
                }
                return instance;
            }
        }

        protected virtual void Init()
        {
            
        }
    }
}