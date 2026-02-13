using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        public float speed;
        public float airborneControlReductionFactor;
        public float jumpForce;
        public float lookSensitivity;

        public GameObject playerCamera;


        private InputAction _move;
        private InputAction _jump;
        private InputAction _look;
        private Rigidbody _rb;
        private bool _jumpTriggered;
        private bool _grounded;

        public void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _move = InputSystem.actions.FindAction("Move");
            _jump = InputSystem.actions.FindAction("Jump");
            _look = InputSystem.actions.FindAction("Look");
            _rb = GetComponent<Rigidbody>();
        }

        public void Update()
        {
            if (_jump.triggered)
                _jumpTriggered = true;
        }

        public void FixedUpdate()
        {
            var moveValue = _move.ReadValue<Vector2>();
            var horizontalMovement = moveValue * (speed * Time.fixedDeltaTime);
            if (!_grounded)
                horizontalMovement *= airborneControlReductionFactor;
            var newPosition = _rb.position + horizontalMovement.y * transform.forward + horizontalMovement.x * transform.right;
            _rb.MovePosition(newPosition);


            var lookValue = _look.ReadValue<Vector2>() * -1;
            var newRotation =
                _rb.rotation * Quaternion.Euler(0, -lookValue.x * lookSensitivity * Time.fixedDeltaTime, 0);
            _rb.MoveRotation(newRotation);

            var newCameraRotation = playerCamera.transform.rotation *
                                    Quaternion.Euler(lookValue.y * lookSensitivity * Time.fixedDeltaTime, 0, 0);
            playerCamera.transform.rotation = newCameraRotation;


            if (_jumpTriggered && _grounded)
            {
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _grounded = false;
            }

            _jumpTriggered = false;
        }

        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Floor"))
            {
                _grounded = true;
            }
        }

        public void Die()
        {
            _rb.Move(new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}