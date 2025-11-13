using UnityEngine;

namespace DrillGame
{
    public class UI_DevGrid : MonoBehaviour
    {
        [SerializeField]
        GameObject grid;
        
        public void ToggleDevButtonActive()
        {
            grid.SetActive(!grid.activeSelf);
        }
    }
}
