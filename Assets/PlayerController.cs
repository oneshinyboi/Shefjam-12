using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        public float speed;
        public float jumpForce;

        private InputAction _move;
        private InputAction _jump;
        private Rigidbody _rb;

        public void Awake()
        {
            _move = InputSystem.actions.FindAction("Move");
            _jump = InputSystem.actions.FindAction("Jump");
            _rb = GetComponent<Rigidbody>();
        }

        public void Update()
        {
            var input = _move.ReadValue<Vector2>();
            var horizontalMovement = input * (speed * Time.deltaTime);
            var newPosition = _rb.position + new Vector3(horizontalMovement.x, 0, horizontalMovement.y);
            _rb.MovePosition(newPosition);

            if (_jump.triggered)
            {
                _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Force);
            }
        }
    }
}