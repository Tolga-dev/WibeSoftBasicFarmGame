using UnityEngine;

namespace Entity
{
    // not used!
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D _rigidbody2D;
        public float speed;

        public void Update()
        {
            var inputh = Input.GetAxisRaw("Horizontal");
            var inputv = Input.GetAxisRaw("Vertical");

            _rigidbody2D.velocity = new Vector2(inputh, inputv) * speed;
        }
    }
}