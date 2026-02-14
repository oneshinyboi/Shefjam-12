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
        public float lookXSensitivity;
        public float lookYSensitivity;

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
            var horizontalMovementVelocity = moveValue * speed;
            if (!_grounded)
                horizontalMovementVelocity *= airborneControlReductionFactor;
            var desiredVelocity = horizontalMovementVelocity.x * transform.right + horizontalMovementVelocity.y * transform.forward;
            desiredVelocity.y += _rb.linearVelocity.y;
            _rb.linearVelocity = desiredVelocity;

            var lookValue = _look.ReadValue<Vector2>();
            Debug.Log(lookValue);

            _rb.angularVelocity = new Vector3(_rb.angularVelocity.x, lookValue.x * lookXSensitivity, _rb.angularVelocity.z);


            var newCameraRotation = playerCamera.transform.rotation *
                                    Quaternion.Euler(-lookValue.y * lookYSensitivity* Time.deltaTime , 0, 0);
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
            _rb.position = Vector3.zero;
            _rb.rotation = Quaternion.identity;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
}