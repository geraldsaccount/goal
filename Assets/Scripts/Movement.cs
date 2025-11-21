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
        float xVelocity = _rigidbody.linearVelocityX;

        bool hasInput = Mathf.Abs(_moveInput) > 0.01f;
        bool moving = Mathf.Abs(xVelocity) > 0.01f;

        if (hasInput && moving && Mathf.Sign(_moveInput) != Mathf.Sign(xVelocity))
        {
            ApplyBraking(xVelocity);
            return;
        }

        if (hasInput)
        {
            if (Mathf.Abs(xVelocity) < _maxSpeed)
            {
                _rigidbody.AddForce(Vector2.right * (_moveInput * _acceleration), ForceMode2D.Force);
            }
            return;
        }

        if (!hasInput && moving)
        {
            ApplyBraking(xVelocity);
        }
    }


    private void ApplyBraking(float xVelocity)
    {
        if (Mathf.Abs(xVelocity) < 0.05f)
        {
            _rigidbody.linearVelocityX = 0;
            return;
        }

        float decelDirection = -Mathf.Sign(xVelocity);
        float maxBrakeForce = _deceleration;

        float neededForce = Mathf.Abs(xVelocity) / Time.fixedDeltaTime;
        float brakeForce = Mathf.Min(maxBrakeForce, neededForce);

        _rigidbody.AddForce(Vector2.right * (brakeForce * decelDirection), ForceMode2D.Force);
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.9f, 0, Vector2.down, _groundCheckDistance, _groundLayers);
        return hit.collider != null;
    }
}
