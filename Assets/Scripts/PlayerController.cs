using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float lookSensitivity;

    public GameObject playerCamera;

    public Rigidbody botRb;

    private bool linkEnabled = false;

    private InputAction _move;
    private InputAction _jump;
    private InputAction _look;
    private InputAction _link;

    private Rigidbody _rb;

    private bool jumpPressed;
    private bool linkPressed;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _move = InputSystem.actions.FindAction("Move");
        _jump = InputSystem.actions.FindAction("Jump");
        _look = InputSystem.actions.FindAction("Look");
        _link = InputSystem.actions.FindAction("Link");

        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_jump.triggered)
        {
            jumpPressed = true;
        }

        if (_link.triggered)
        {
            linkPressed = true;
        }
    }

    public void FixedUpdate()
    {
        if (linkPressed)
        {
            linkEnabled = !linkEnabled;
            linkPressed = false;
        }

        var moveValue = _move.ReadValue<Vector2>();
        var horizontalMovement = moveValue * (speed * Time.fixedDeltaTime);

        // Move PLAYER
        Vector3 newPosition = _rb.position +
                              horizontalMovement.y * transform.forward +
                              horizontalMovement.x * transform.right;

        _rb.MovePosition(newPosition);

        // Move BOT if linked
        if (linkEnabled && botRb != null)
        {
            Vector3 botNewPosition = botRb.position +
                                     horizontalMovement.y * botRb.transform.forward +
                                     horizontalMovement.x * botRb.transform.right;

            botRb.MovePosition(botNewPosition);
        }

        // Rotation
        var lookValue = _look.ReadValue<Vector2>() * -1;

        Quaternion newRotation =
            _rb.rotation * Quaternion.Euler(0, -lookValue.x * lookSensitivity * Time.fixedDeltaTime, 0);

        _rb.MoveRotation(newRotation);

        if (linkEnabled && botRb != null)
        {
            botRb.MoveRotation(Quaternion.Euler(0, _rb.rotation.eulerAngles.y, 0));
        }

        if (jumpPressed)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (linkEnabled && botRb != null)
            {
                botRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            jumpPressed = false;
        }
    }

    public void Die()
    {
        _rb.Move(new Vector3(0, 0, 0), Quaternion.identity);
    }
}
