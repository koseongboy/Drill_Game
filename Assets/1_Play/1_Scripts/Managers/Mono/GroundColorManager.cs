using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DrillGame.Managers
{
    public class GroundColorManager : MonoBehaviour
    {
        #region Singleton & initialization
        public static GroundColorManager Instance { get; private set; }
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
        }
        #endregion
        
        #region Fields & Properties
        [SerializeField] private Tilemap backgroundBaseTilemap;
        [SerializeField] private Tilemap backgroundTilemap;
        [SerializeField] private SpriteRenderer ground;
        [SerializeField] private ParticleSystem groundParticleSystem;
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        
        public void ChangeTilemapColor(Color color)
        {
            backgroundBaseTilemap.color = color;
            backgroundTilemap.color = color;
            ground.material.color = color;
            var main = groundParticleSystem.main;
            main.startColor = color;
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
        
        #region DEV
        #endregion
    }
}