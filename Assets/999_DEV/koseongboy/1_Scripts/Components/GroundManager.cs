using System.Collections;
using System.Collections.Generic;
using DrillGame.Components;
using UnityEngine;

namespace DrillGame.Managers
{
    public class GroundManager : MonoBehaviour
    {
#if UNITY_EDITOR

        public void ButtonClicked()
        {

            GiveDamage(1);
        }
#endif
        #region Fields & Properties
        public GameObject currentGroundPrefab;
        public GameObject nextGroundPrefab;

        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void SpawnNewGround()
        {
            currentGroundPrefab = Instantiate(nextGroundPrefab, transform.position, Quaternion.identity);
            nextGroundPrefab = DataLoadManager.Instance.GetGroundPrefab();
        }
        public void GiveDamage(int damage)
        {
            if (currentGroundPrefab.GetComponent<GroundController>().TakeDamage(damage))
            {
                Debug.Log("Ground Destroyed");
                Debug.Log("Instantiate New Ground");
                Destroy(currentGroundPrefab);
                SpawnNewGround();
            }
        }
        #endregion

        #region private methods

        #endregion

        #region Unity event methods
        private void Awake()
        {
            SpawnNewGround();
            if (nextGroundPrefab == null || currentGroundPrefab == null)
            {
                Debug.LogError("GroundManager: nextGroundPrefab or currentGroundPrefab is not assigned!");
            }
        }

        private void Start()
        {

        }

        private void Update()
        {

        }
    private void OnDestroy()
    {
        if (GroundPrefabLoader.Instance != null)
        {
            GroundPrefabLoader.Instance.ReleaseGroundPrefab();
            Debug.Log("Ground prefab released on GroundManager destroy.");
        }
    }
    #endregion
  }
}