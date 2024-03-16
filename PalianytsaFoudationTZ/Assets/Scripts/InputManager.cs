using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Rigidbody rb;
    private GameInput gameInput;
    private Vector2 moveInput;
    private bool jumpInput;
    private bool isGrounded;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameInput = new GameInput();
    }

    private void OnEnable()
    {
        gameInput.Enable();
        gameInput.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        gameInput.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        gameInput.Player.Jump.performed += ctx => jumpInput = true;
    }

    private void OnDisable()
    {
        gameInput.Disable();
    }

    private void FixedUpdate()
    {
        Move();

        if (jumpInput)
        {
            Jump();
            jumpInput = false; 
        }
    }

    private void Move()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + transform.TransformDirection(movement));
    }

    private void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpdate(collision, true);
    }

    private void OnCollisionExit(Collision collision)
    {
        IsGroundedUpdate(collision, false);
    }

    private void IsGroundedUpdate(Collision collision, bool value)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = value;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
