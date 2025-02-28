using UnityEngine;

namespace Scenes.farm_game_tutorial
{
    public class IsometricZ : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 100);   
        }
    }
}
