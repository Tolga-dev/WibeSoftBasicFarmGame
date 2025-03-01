using UnityEngine;

namespace Managers
{
    public class ManagerBase : MonoBehaviour
    {
        public GameManager gameManager;

        public void Awake()
        {
            gameManager = GameManager.Instance;
        }
    }
}