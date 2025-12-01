using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace DrillGame._1_Play._1_Scripts.Managers.Mono
{

    public class ItemSlotPoolManager : MonoBehaviour
    {
        #region Singleton & initialization
        public static ItemSlotPoolManager Instance { get; private set; }
        
        // ⭐ ObjectPool 인스턴스
        private IObjectPool<GameObject> pool; 

        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform slotObjectParent;
        
        private const int DefaultPoolSize = 14;
        private const int MaxPoolSize = 80;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }

            pool = new ObjectPool<GameObject>(
                CreatePooledItem,       // 1. 객체 생성 함수 (필수)
                OnGetItem,              // 2. 객체를 가져올 때 동작
                OnReleaseItem,          // 3. 객체를 반환할 때 동작
                OnDestroyItem,          // 4. 최대 크기를 초과하여 파괴할 때 동작
                collectionCheck: true,  // 이중 반환(Double-Return) 체크 활성화 (안정성)
                defaultCapacity: DefaultPoolSize, // 초기 생성 크기
                maxSize: MaxPoolSize    // 최대 크기
            );
            
            // 초기 풀 사이즈만큼 객체를 미리 생성 (Warm-up)
            PrewarmPool(DefaultPoolSize);
        }
        #endregion

        // 풀에서 객체를 가져오는 메서드
        public GameObject Get()
        {
            return pool.Get();
        }
        
        // 풀에 객체를 반환하는 메서드
        public void Return(GameObject slot)
        {
            if (slot == null)
            {
                // UI_Inventory에서 발생했던 MissingReferenceException 방지 코드 유지
                return;
            }
            
            // ⭐ pool.Release() 호출로 단순화 (크기 체크 로직 불필요)
            pool.Release(slot);
        }
        
        // --- ObjectPool이 요구하는 4가지 델리게이트 메서드 ---
        
        private GameObject CreatePooledItem()
        {
            var obj = Instantiate(slotPrefab, slotObjectParent);
            return obj;
        }

        private void OnGetItem(GameObject slot)
        {
            slot.SetActive(true); 
        }

        private void OnReleaseItem(GameObject slot)
        {
            slot.SetActive(false); 
        }

        private void OnDestroyItem(GameObject slot)
        {
            Debug.Log("너냐?");
            Destroy(slot); 
        }
        
        
        private void PrewarmPool(int count)
        {
            var items = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                // Get() 호출로 CreatePooledItem -> OnGetItem이 실행됨
                items.Add(pool.Get());
            }
            
            foreach (var item in items)
            {
                // Release() 호출로 OnReleaseItem이 실행되어 비활성화됨
                pool.Release(item);
            }
        }
    }
}