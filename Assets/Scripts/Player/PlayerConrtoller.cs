using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerConrtoller : MonoBehaviour
{
    [SerializeField]private float _speed = 5f;
    public static PlayerConrtoller Instance { get; private set; }
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private float _minSpeed = 0.01f;

    private bool _isRunning = false;
    private Vector2 _lastDirection = Vector2.down;

    private PlayerInputAction _inputAction;
    public bool IsRunning()
    {
        return _isRunning;
    }
    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
        _inputAction = new PlayerInputAction();
    }
    private void OnEnable()
    {
        _inputAction.Player.Enable();
        _inputAction.Player.Move.performed += OnMove;
        _inputAction.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        _inputAction.Player.Move.performed -= OnMove;
        _inputAction.Player.Move.canceled -= OnMove;
        _inputAction.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>().normalized;

        if (_moveInput != Vector2.zero)
        {
            _lastDirection = _moveInput;
        }
    }
    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveInput * _speed * Time.fixedDeltaTime);
        _isRunning = _moveInput.sqrMagnitude > 0.01f;
        
    }

    public Vector2 GetLastDirection() => _lastDirection;
}
