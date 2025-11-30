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

            InitQueue();
        }
        #endregion
        
        private Queue<GameObject> slotPool;

        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform slotObjectParent;
        
        private const int DefaultPoolSize = 14;
        private const int MaxPoolSize = 80;
        
        private void InitQueue()
        {
            slotPool = new Queue<GameObject>();
            for (int i = 0; i < DefaultPoolSize; i++)
            {
                var obj = Instantiate(slotPrefab, slotObjectParent);
                obj.SetActive(false);
                slotPool.Enqueue(obj);
            }
        }
        
        public GameObject Get()
        {
            GameObject slot;
            if (slotPool.Count >= 0)
            {
                slot = slotPool.Dequeue();
                slot.SetActive(true);
            }
            else
            {
                slot = Instantiate(slotPrefab, slotObjectParent);
            }
            return slot;
        }
        
        public void Return(GameObject slot)
        {
            if (slotPool.Count + 1 <= MaxPoolSize)
            {
                slot.SetActive(false);
                slotPool.Enqueue(slot);
            }
            else
            {
                Destroy(slot);
            }
        }
    }
}