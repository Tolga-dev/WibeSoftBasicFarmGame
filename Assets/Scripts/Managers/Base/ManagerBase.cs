using UnityEngine;

namespace Managers.Base
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