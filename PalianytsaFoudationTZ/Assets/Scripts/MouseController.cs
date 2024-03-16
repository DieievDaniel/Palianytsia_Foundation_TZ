using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    private GameInput gameInput;
    private Vector2 lookInput;
    private Vector2 smoothedLookInput;
    private bool isLooking;
    public float sensitivity = 2f;
    public float smoothing;
    public Transform playerBody;

    private void Awake()
    {
        gameInput = new GameInput();
    }

    private void OnEnable()
    {
        gameInput.Enable();
        gameInput.Player.Look.performed += ctx =>
        {
            lookInput = ctx.ReadValue<Vector2>();
            isLooking = true;
        };
        gameInput.Player.Look.canceled += ctx => isLooking = false;
    }

    private void OnDisable()
    {
        gameInput.Disable();
    }

    private void Start()
    {
        Cursor.visible = false; 
    }

    private void Update()
    {
        if (isLooking)
        {
            smoothedLookInput = Vector2.Lerp(smoothedLookInput, lookInput * sensitivity * Time.deltaTime, smoothing * Time.deltaTime);

            float mouseX = smoothedLookInput.x;

            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
