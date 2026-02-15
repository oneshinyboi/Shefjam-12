using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float airborneControlReductionFactor;
    public float jumpForce;
    public float lookXSensitivity;
    public float lookYSensitivity;

    public GameObject playerCamera;
    public GameObject respawnPoint;
    public Rigidbody botRb;
    private Vector3 _respawnPosition; // Respawn position (set in the Inspector)

    private InputAction _move;
    private InputAction _jump;
    private InputAction _look;
    private InputAction _link;
    private Rigidbody _rb;
    private bool _jumpTriggered = false;
    private bool _linkTriggered = false;
    private bool _grounded;
    private bool _botGrounded;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _move = InputSystem.actions.FindAction("Move");
        _jump = InputSystem.actions.FindAction("Jump");
        _look = InputSystem.actions.FindAction("Look");
        _link = InputSystem.actions.FindAction("Link");
        _rb = GetComponent<Rigidbody>();
        _respawnPosition = respawnPoint.transform.position;
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
        _grounded = ControlMovement(_rb, _grounded);

        var lookValue = _look.ReadValue<Vector2>();
        // Camera rotation
        var newCameraRotation = playerCamera.transform.rotation *
                                Quaternion.Euler(-lookValue.y * lookYSensitivity * Time.deltaTime, 0, 0);

        playerCamera.transform.rotation = newCameraRotation;

        if (_linkTriggered && botRb != null)
        {
            _botGrounded = ControlMovement(botRb, _botGrounded);
        }

    }

    public bool ControlMovement(Rigidbody rb, bool grounded)
    {
        var moveValue = _move.ReadValue<Vector2>();
        var horizontalAcceleration = moveValue * acceleration;

        if (!_grounded)
            horizontalAcceleration *= airborneControlReductionFactor; // make moving while in the air more rigid

        var thisFramesVelocity = horizontalAcceleration.x * rb.transform.right * Time.deltaTime + horizontalAcceleration.y * rb.transform.forward * Time.deltaTime;
        rb.linearVelocity += thisFramesVelocity;
        var horizontalVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            var scaledHorizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            Vector3 limitedVelocity =
                new Vector3(scaledHorizontalVelocity.x, rb.linearVelocity.y, scaledHorizontalVelocity.y);
            rb.linearVelocity = limitedVelocity;
        }

        var lookValue = _look.ReadValue<Vector2>();

        var angularVelocityToAdd = new Vector3(0, lookValue.x * lookXSensitivity * Time.deltaTime, 0);
        rb.angularVelocity += angularVelocityToAdd;

        // Jumping
        if (_jumpTriggered && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            grounded = false;
        }

        _jumpTriggered = false;
        return grounded;

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
        _rb.position = _respawnPosition; // Respawn the player to a specified position
        _rb.rotation = Quaternion.identity;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}

