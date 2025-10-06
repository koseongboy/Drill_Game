using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrillGame
{
    public class SceneCaller_forDev : MonoBehaviour
    {
        #region Fields & Properties
        IEnumerator sceneCallCoroutine()
        {
            yield return new WaitForSeconds(3.0f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Play_koseongboy");
        }
        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        private void Awake()
        {

        }

        private void Start()
        {
            StartCoroutine(sceneCallCoroutine());
        }

        private void Update()
        {
        
        }
        #endregion
    }
}