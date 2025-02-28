using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets_DevilsWorkShop.Test
{
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerAction playerAction;
        public Tilemap map;
        public Vector3 destination;
        public float movementSpeed = 5 ;

        private void Awake()    
        {
            playerAction = new PlayerAction();
        }

        public void OnDisable()
        {
            playerAction.Disable();
        }
        public void OnEnable()
        {
            playerAction.Enable();
        }
        
        private void Start()
        {
            destination = transform.position;
            playerAction.PlayerInput.Mouse.performed += ctx => Move();
        }

        private void Move()
        {
            var pos = playerAction.PlayerInput.MousePos.ReadValue<Vector2>();
            pos = Camera.main.ScreenToWorldPoint(pos);

            var gridPos = map.WorldToCell(pos);

            if (map.HasTile(gridPos))
            {
                destination = pos;
            }
        }

        private void Update()
        {
            if(Vector3.Distance(transform.position, destination) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed* Time.deltaTime);
            }
            
        }
    }
}