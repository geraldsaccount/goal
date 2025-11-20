using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField, Range(0,1)] private float _shortHopMultiplier = 0.5f;
    [SerializeField] private LayerMask _groundLayers;

    private Rigidbody2D _rigidbody;

    private float _moveInput;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>().x;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if(CheckGround())
                {
                    _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                }
                break;
            case InputActionPhase.Canceled:
                if(!CheckGround() && _rigidbody.linearVelocityY > 0)
                {
                    _rigidbody.linearVelocityY *= _shortHopMultiplier;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (_moveInput != 0 && Mathf.Abs(_rigidbody.linearVelocityX) < _maxSpeed)
        {
            _rigidbody.AddForceX(_acceleration * _moveInput);
        }
        if(_moveInput == 0 || Mathf.Sign(_rigidbody.linearVelocityX) != Mathf.Sign(_moveInput))
        {
            if (Mathf.Sign(_rigidbody.linearVelocityX) > 0)
            {
                _rigidbody.AddForceX(Mathf.Min(_deceleration * -_rigidbody.linearVelocityX, -_rigidbody.linearVelocityX));
            }
            else
            {
                _rigidbody.AddForceX(Mathf.Max(_deceleration * -_rigidbody.linearVelocityX, -_rigidbody.linearVelocityX));
            }
        }
        
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.9f, 0, Vector2.down, _groundCheckDistance, _groundLayers);
        return hit.collider != null;
    }
}
