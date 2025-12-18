using UnityEngine;

namespace DrillGame
{
    public class UI_DevGrid : MonoBehaviour
    {
        [SerializeField]
        GameObject grid;
        
        public void ToggleDevButtonActive()
        {
            return;
            grid.SetActive(!grid.activeSelf);
        }
    }
}
