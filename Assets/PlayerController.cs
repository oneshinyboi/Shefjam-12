using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float airborneControlReductionFactor;
    public float jumpForce;
    public float lookXSensitivity;
    public float lookYSensitivity;

    public GameObject playerCamera;
    public Rigidbody _botRb;
    public Vector3 respawnPosition; // Respawn position (set in the Inspector)

    private InputAction _move;
    private InputAction _jump;
    private InputAction _look;
    private InputAction _link;
    private Rigidbody _rb;
    private bool _jumpTriggered = false;
    private bool _linkTriggered = false;
    private bool _grounded;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _move = InputSystem.actions.FindAction("Move");
        _jump = InputSystem.actions.FindAction("Jump");
        _look = InputSystem.actions.FindAction("Look");
        _link = InputSystem.actions.FindAction("Link");
        _rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (_jump.triggered)
        {
            _jumpTriggered = true;
        }

        if (_link.triggered)
        {
            _linkTriggered = !_linkTriggered;
        }
    }

    public void FixedUpdate()
    {
        var moveValue = _move.ReadValue<Vector2>();
        var horizontalMovementVelocity = moveValue * speed;

        if (!_grounded)
            horizontalMovementVelocity *= airborneControlReductionFactor; // make moving while in the air more rigid

        var desiredVelocity = horizontalMovementVelocity.x * transform.right + horizontalMovementVelocity.y * transform.forward;
        desiredVelocity.y += _rb.linearVelocity.y;
        _rb.linearVelocity = desiredVelocity;

        var lookValue = _look.ReadValue<Vector2>();

        _rb.angularVelocity = new Vector3(_rb.angularVelocity.x, lookValue.x * lookXSensitivity, _rb.angularVelocity.z);

        // Camera rotation
        var newCameraRotation = playerCamera.transform.rotation *
                                Quaternion.Euler(-lookValue.y * lookYSensitivity * Time.deltaTime, 0, 0);

        playerCamera.transform.rotation = newCameraRotation;

        if (_linkTriggered && _botRb != null)
        {
            desiredVelocity.y += _rb.linearVelocity.y;
            _botRb.linearVelocity = desiredVelocity;

            _botRb.angularVelocity = _rb.angularVelocity;
            Debug.Log(_botRb.angularVelocity);
        }

        // Jumping
        if (_jumpTriggered && _grounded)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _grounded = false;
        }

        _jumpTriggered = false;
    }

    // Collision detection for grounding
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            _grounded = true;
        }
    }

    // Die and respawn at the designated position
    public void Die()
    {
        _rb.position = respawnPosition; // Respawn the player to a specified position
        _rb.rotation = Quaternion.identity;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}

